using System.Diagnostics;

namespace FallingBlocks
{
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