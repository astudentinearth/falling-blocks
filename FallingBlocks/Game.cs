using System.Diagnostics;

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
}
