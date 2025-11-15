namespace Spectre.Tui;

public interface IRendererContext
{
    Rectangle Viewport { get; }

    void Render(IWidget widget);
    void Render(IWidget widget, Rectangle area);

    /// <summary>
    /// Sets the rune at the specified (viewport) coordinates.
    /// </summary>
    void SetRune(int x, int y, Rune rune);
}

public static class IRenderContextExtensions
{
    extension(IRendererContext context)
    {
        public void SetRune(int x, int y, char rune)
        {
            context.SetRune(x, y, new Rune(rune));
        }
    }
}

internal class RenderContext : IRendererContext
{
    public Buffer Buffer { get; }
    public Rectangle Screen { get; }
    public Rectangle Viewport { get; }

    public RenderContext(Buffer buffer, Rectangle screen, Rectangle viewport)
    {
        Buffer = buffer;
        Screen = screen;
        Viewport = viewport;
    }

    public void Render(IWidget widget)
    {
        widget.Render(this);
    }

    public void Render(IWidget widget, Rectangle area)
    {
        var screen = new Rectangle(Screen.X + area.X, Screen.Y + area.Y, area.Width, area.Height);
        var viewport = new Rectangle(0, 0, screen.Width, screen.Height);
        widget.Render(new RenderContext(Buffer, screen, viewport));
    }

    public void SetRune(int x, int y, Rune rune)
    {
        if (!Viewport.Contains(x, y))
        {
            return;
        }

        Buffer.SetRune(Screen.X + x, Screen.Y + y, rune);
    }
}