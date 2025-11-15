namespace Spectre.Tui;

public sealed class ClearWidget(Rune rune) : IWidget
{
    public void Render(IRendererContext context)
    {
        for (var x = 0; x < context.Viewport.Width; x++)
        {
            for (var y = 0; y < context.Viewport.Height; y++)
            {
                context.SetRune(x, y, rune);
            }
        }
    }
}