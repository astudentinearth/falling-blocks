namespace FallingBlocks
{
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
            for (int i = 0; i < x.Length; i++)
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
            foreach (Point p in points)
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
}