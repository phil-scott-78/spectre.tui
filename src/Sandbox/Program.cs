using Spectre.Tui;

namespace Sandbox;

public static class Program
{
    public static void Main(string[] args)
    {
        var running = true;

        using var terminal = new Terminal();
        var renderer = new Renderer(terminal);

        while (running)
        {
            renderer.Draw((_, _) =>
            {
                // Time to quit?
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    running = false;
                }
            });
        }
    }
}