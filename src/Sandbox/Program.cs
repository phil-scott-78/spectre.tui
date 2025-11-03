using Spectre.Tui;

namespace Sandbox;

public static class Program
{
    public static void Main(string[] args)
    {
        using var terminal = new Terminal();
        var renderer = new Renderer(terminal);

        while (true)
        {
            renderer.Draw((frame, elapsed) =>
            {
            });
        }
    }
}