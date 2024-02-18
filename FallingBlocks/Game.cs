using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FallingBlocks
{
    internal static class Game
    {
        public static bool[,] blocks = new bool[20, 10];
        public static int score = 0;
        private static System.Timers.Timer loop = new System.Timers.Timer();
        private static Block currentBlock;
        internal static void Renderloop()
        {
            currentBlock = new Block_T();
            loop.Elapsed += (sender, e) => {
                if (!currentBlock.MoveDown()) {
                    Block.ClearBlocks();
                    currentBlock = new Block_I();
                };
                Redraw();
            };
            loop.Interval = 150;
            loop.Start();
        }
        internal static void Redraw()
        {
            Renderer.DrawScreen(blocks, score);
        }

        internal static void GameOver()
        {
            loop.Stop();
            Console.WriteLine("Game over");
        }
        public static void HandleKey(ConsoleKeyInfo key)
        {
            Debug.WriteLine(key.Key.ToString());
            if(key.Key == ConsoleKey.RightArrow && loop.Enabled)
            {
                Debug.WriteLine("here");
                currentBlock.MoveX(+1);
            }
            if (key.Key == ConsoleKey.LeftArrow && loop.Enabled)
            {
                Debug.WriteLine("here");
                currentBlock.MoveX(-1);
            }
        }

        public static bool[] GetRow(bool[,] blocks, int rowNumber)
        {
            return Enumerable.Range(0, blocks.GetLength(1)).Select(b => blocks[rowNumber, b]).ToArray();
        }
    }
    internal class Point
    {
        public byte X;
        public byte Y;
        public Point(byte x, byte y)
        {
            X = x;
            Y = y;
        }
        public static Point[] fromArray(byte[] x, byte[] y)
        {
            if (x.Length != y.Length) return Array.Empty<Point>();
            List<Point> points = new List<Point>();
            for(int i = 0; i < x.Length; i++)
            {
                //Debug.WriteLine(string.Format("[POINT::fromArray] Creating point {0} {1}", x[i], y[i]));
                points.Add(new Point(x[i], y[i]));
            }
            return points.ToArray();
        }
    }

    internal static class PointExtensions
    {
        public static bool ContainsPoint(this Point[] points, Point point)
        {
            //Debug.WriteLine(string.Format(">Searching for {0},{1}",point.X, point.Y));
            foreach(Point p in points)
            {

                if (p.isEqual(point)) return true;
            }
            //Debug.WriteLine("----------");
            return false;
        }
        public static bool isEqual(this Point point, Point otherPoint)
        {
            //Debug.WriteLine(string.Format(">>>Comparing against {0},{1}", otherPoint.X, otherPoint.Y));
            return (point.X == otherPoint.X && point.Y == otherPoint.Y);
        }
        public static bool isEqual(this Point point, byte x, byte y)
        {
            return (point.X == x && point.Y == y);
        }
    }

    internal class Block
    {
        public static void ClearBlocks()
        {
            List<int> rows = new();
            // determine rows to clear
            for(int i=19; i > -1; i--)
            {
                var row = Game.GetRow(Game.blocks, i);
                var clear = true;
                foreach(bool x in row)
                {
                    if (!x) {
                        clear = false;
                        break; 
                    }
                }
                if(clear)
                {
                    rows.Add(i);
                }
            }
            if (rows.Count == 0) return;
            // clear rows
            foreach(var i in rows)
            {
                for(byte j = 0; j < 10; j++)
                {
                    Game.blocks[i, j] = false;
                }
            }
            // move down the blocks
            bool[,] newBlocks = new bool[20, 10];
            for(int i = 0; i<20; i++)
            {
                // determine collapse distance
                int dist = 0;
                foreach(var j in rows)
                {
                    if (i < j) dist++;
                }

                // copy the row to row i+dist
                var row = Game.GetRow(Game.blocks,i);
                for(int j = 0; j<10; j++)
                {
                    newBlocks[i+dist, j] = row[j];
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
            foreach(Point p in points)
            {
                //Debug.WriteLine(string.Format("[BLOCK::MoveDown] Checking point {0} {1}", p.X, p.Y));
                Point below = new Point(p.X, (byte)(p.Y + 1));
                //Debug.WriteLine(string.Format("Below point: {0} {1}", below.X, below.Y));
                
                if (points.ContainsPoint(below)) continue; // if its still ours we dont care
                
                //Debug.WriteLine("Did we hit the floor? ", (below.Y > 19).ToString());
                if (below.Y > 19){
                    return false; 
                } // we have hit the floor.

                //Debug.WriteLine("Is the below point occupied? ", (Game.blocks[below.Y, below.X]).ToString());
                if (Game.blocks[below.Y, below.X]==true) return false; // if the block is full we land.
                
            }
            List<Point> newPoints = new();
            foreach(Point p in points)
            {
                Point newPoint = new(p.X, (byte)(p.Y + 1));
                Game.blocks[p.Y, p.X] = false; // free old position
                newPoints.Add(newPoint);
            }
            points = newPoints.ToArray();
            foreach(Point p in points)
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
                Point off = new Point((byte)(p.X+offset),p.Y);
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
                Point newPoint = new((byte)(p.X+offset),p.Y);
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

    internal class Block_T : Block
    {
        public Block_T()
        {
            points = Point.fromArray([3, 4, 5, 4], [0, 0, 0, 1]);
            Debug.WriteLine(points.Length);
            origin = new Point(4, 0);
            TrySpawn();
        }
    }

    internal class Block_I : Block
    {
        public Block_I()
        {
            points = Point.fromArray([0, 0, 0, 0], [0, 1, 2, 3]);
            Debug.WriteLine(points.Length);
            origin = new Point(4, 0);
            TrySpawn();
        }
    }
}
