using System.Timers;
using FallingBlocks;
internal class Program
{

    private static void Main(string[] args)
    {
        Game.Renderloop();
        while (true)
        {
            var key = Console.ReadKey();
            Game.HandleKey(key);
        }
    }
}