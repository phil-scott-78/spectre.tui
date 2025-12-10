using System.Text;
using Spectre.Tui;

namespace Sandbox;

public sealed class ClearWidget(
    Rune rune,
    Decoration decoration = Decoration.None,
    Color? foreground = null,
    Color? background = null) : IWidget
{
    public void Render(IRenderContext context)
    {
        for (var x = 0; x < context.Viewport.Width; x++)
        {
            for (var y = 0; y < context.Viewport.Height; y++)
            {
                context.SetCell(x, y, new Cell
                {
                    Decoration = decoration,
                    Rune = rune,
                    Foreground = foreground ?? Color.Default,
                    Background = background ?? Color.Default,
                });
            }
        }
    }
}