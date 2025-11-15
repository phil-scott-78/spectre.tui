using System.Text;
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
            renderer.Draw((ctx, _) =>
            {
                // Clear the background
                ctx.Render(new ClearWidget(new Rune('▄')));

                // Render a box
                var inner = ctx.Viewport.Inflate(new Size(-5, -5));
                ctx.Render(new BoxWidget(), inner);
            });

            // Time to quit?
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
            {
                running = false;
            }
        }
    }
}