using Spectre.Tui;

namespace Sandbox;

public sealed class TextWidget(
    string text,
    Color? foreground = null,
    Color? background = null) : IWidget
{
    private string _text = text ?? throw new ArgumentNullException(nameof(text));

    public void Render(IRenderContext context)
    {
        _text = $" FPS: {_text} ";

        var x = (context.Viewport.Width - _text.Length) / 2;
        var y = context.Viewport.Height / 2;

        foreach (var rune in _text)
        {
            context.SetRune(x, y, rune);
            context.SetForeground(x, y, foreground);
            context.SetBackground(x, y, background);
            x++;
        }
    }
}
