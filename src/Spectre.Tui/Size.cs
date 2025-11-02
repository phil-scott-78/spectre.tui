namespace Spectre.Tui;

public readonly struct Size(int width, int height)
{
    public int Width { get; } = width;
    public int Height { get; } = height;

    public int Area => Width * Height;

    public Region ToRegion()
    {
        return new Region(0, 0, Width, Height);
    }
}

public readonly struct Position(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
}