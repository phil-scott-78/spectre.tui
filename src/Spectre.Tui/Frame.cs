namespace Spectre.Tui;

public sealed class Frame
{
    public Buffer Buffer { get; }
    public Region ViewPort => Buffer.Region;

    public Frame(Buffer buffer)
    {
        Buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
    }

    public void Render<T>(T widget)
        where T : IWidget
    {
        Buffer.Render(widget, ViewPort);
    }

    public void Render<T, TState>(T widget, TState state)
        where T : IWidget<TState>
    {
        Buffer.Render(widget, ViewPort, state);
    }
}