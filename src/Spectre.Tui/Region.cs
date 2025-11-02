namespace Spectre.Tui;

public readonly struct Region(int x, int y, int width, int height)
    : IEquatable<Region>
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public int Width { get; } = width;
    public int Height { get; } = height;

    public int Top => Y;
    public int Bottom => Y + Height;
    public int Left => X;
    public int Right => X + Width;

    public int Area => Width * Height;

    public bool Equals(Region other)
    {
        return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
    }

    public override bool Equals(object? obj)
    {
        return obj is Region other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Width, Height);
    }

    public override string ToString()
    {
        return $"{X},{Y},{Width},{Height}";
    }
}