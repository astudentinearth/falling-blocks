using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallingBlocks
{
    internal static class Renderer
    {
        public static void DrawScreen(bool[,] blocks, int score)
        {
            Debug.WriteLine("[RENDERER] Redrawing screen");
            List<string> lines = new();
            lines.Add("SCORE : "+score.ToString());
            for(int i=0; i<20; i++)
            {
                lines.Add(" |" + RowToString(Game.GetRow(blocks, i)) + "|");
            }
            lines.Add(" +--------------------+");
            Console.Clear();
            foreach (string l in lines) Console.WriteLine(l);
        }

        public static string RowToString(bool[] blocks)
        {
            string line = "";
            foreach(bool b in blocks)
            {
                line += (b ? "██" : "  ");
            }
            return line;
        }
    }
}
