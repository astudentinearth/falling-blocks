using System.Timers;
internal class Program
{
    private static int count = 0;
    private static System.Timers.Timer loop = new System.Timers.Timer();
    private static void Main(string[] args)
    {
        Renderloop();
        Console.ReadKey();
    }

    private static void Renderloop()
    {
        loop.Elapsed += (sender, e) => { count++; Redraw(); Console.WriteLine("Hello world"); };
        loop.Interval = 250;
        loop.Start();
    }
    public static void Redraw()
    {
        Console.Clear();
        Console.WriteLine(count.ToString());
    }
}