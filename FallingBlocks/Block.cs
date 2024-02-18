using FallingBlocks;
using System.Diagnostics;

namespace FallingBlocks
{
    internal class Block
    {
        public static void ClearBlocks()
        {
            List<int> rows = new();
            // determine rows to clear
            for (int i = 19; i > -1; i--)
            {
                var row = Game.GetRow(Game.blocks, i);
                var clear = true;
                foreach (bool x in row)
                {
                    if (!x)
                    {
                        clear = false;
                        break;
                    }
                }
                if (clear)
                {
                    rows.Add(i);
                }
            }
            if (rows.Count == 0) return;
            // clear rows
            foreach (var i in rows)
            {
                for (byte j = 0; j < 10; j++)
                {
                    Game.blocks[i, j] = false;
                }
            }
            // move down the blocks
            bool[,] newBlocks = new bool[20, 10];
            for (int i = 0; i < 20; i++)
            {
                // determine collapse distance
                int dist = 0;
                foreach (var j in rows)
                {
                    if (i < j) dist++;
                }

                // copy the row to row i+dist
                var row = Game.GetRow(Game.blocks, i);
                for (int j = 0; j < 10; j++)
                {
                    newBlocks[i + dist, j] = row[j];
                }
            }
            // replace the blocks
            Game.blocks = newBlocks;

            // add score
            Game.score += 100 * (4 ^ rows.Count);

            Game.Redraw();
        }
        public Point[] points = new Point[4];
        public Point origin;
        public bool MoveDown()
        {
            //Debug.WriteLine("!! BEGIN FALL");
            //Debug.WriteLine(string.Format("||||This block owns these points: [{0},{1}] [{2},{3}] [{4},{5}] [{6},{7}]"
            //    , points[0].X, points[0].Y, points[1].X, points[1].Y, points[2].X, points[2].Y, points[3].X, points[3].Y));
            // check if we can move
            foreach (Point p in points)
            {
                //Debug.WriteLine(string.Format("[BLOCK::MoveDown] Checking point {0} {1}", p.X, p.Y));
                Point below = new Point(p.X, (byte)(p.Y + 1));
                //Debug.WriteLine(string.Format("Below point: {0} {1}", below.X, below.Y));

                if (points.ContainsPoint(below)) continue; // if its still ours we dont care

                //Debug.WriteLine("Did we hit the floor? ", (below.Y > 19).ToString());
                if (below.Y > 19)
                {
                    return false;
                } // we have hit the floor.

                //Debug.WriteLine("Is the below point occupied? ", (Game.blocks[below.Y, below.X]).ToString());
                if (Game.blocks[below.Y, below.X] == true) return false; // if the block is full we land.

            }
            List<Point> newPoints = new();
            foreach (Point p in points)
            {
                Point newPoint = new(p.X, (byte)(p.Y + 1));
                Game.blocks[p.Y, p.X] = false; // free old position
                newPoints.Add(newPoint);
            }
            points = newPoints.ToArray();
            foreach (Point p in points)
            {
                Game.blocks[p.Y, p.X] = true; // occupy new position
            }
            Debug.WriteLine("!! END FALL");
            return true;
        }

        public bool MoveX(int offset)
        {
            foreach (Point p in points)
            {
                //Debug.WriteLine(string.Format("[BLOCK::MoveDown] Checking point {0} {1}", p.X, p.Y));
                Point off = new Point((byte)(p.X + offset), p.Y);
                //Debug.WriteLine(string.Format("Below point: {0} {1}", below.X, below.Y));

                if (points.ContainsPoint(off)) continue; // if its still ours we dont care

                //Debug.WriteLine("Did we hit the wall? ", (below.Y > 19).ToString());
                if (off.X > 9 || off.X < 0) return false; // we have hit the wall.

                //Debug.WriteLine("Is the below point occupied? ", (Game.blocks[below.Y, below.X]).ToString());
                if (Game.blocks[off.Y, off.X] == true) return false; // if the block is full we land.

            }
            List<Point> newPoints = new();
            foreach (Point p in points)
            {
                Point newPoint = new((byte)(p.X + offset), p.Y);
                Game.blocks[p.Y, p.X] = false; // free old position
                newPoints.Add(newPoint);
            }
            points = newPoints.ToArray();
            foreach (Point p in points)
            {
                Game.blocks[p.Y, p.X] = true; // occupy new position
            }
            Game.Redraw();
            return true;
        }
        public void TrySpawn()
        {
            foreach (Point p in points)
            {
                // if the spawn location is occupied end the game
                if (Game.blocks[p.Y, p.X])
                {
                    Game.GameOver();
                    break;
                }
            }
            foreach (Point p in points)
            {
                Game.blocks[p.Y, p.X] = true;
            }
        }
    }
}