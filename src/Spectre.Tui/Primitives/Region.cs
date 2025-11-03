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

    public bool Contains(Position position)
    {
        return position.X >= X && position.X <= X + Width &&
               position.Y >= Y && position.Y <= Y + Height;
    }

    public bool Intersects(Region value)
    {
        return value.Left < Right && Left < value.Right &&
               value.Top < Bottom && Top < value.Bottom;
    }

    public Region Inflate(Size size)
    {
        return Inflate(size.Width, size.Height);
    }

    public Region Inflate(int width, int height)
    {
        return new Region(
            x: X - width,
            y: Y - height,
            width: Width + (2 * width),
            height: Height + (2 * height)
        );
    }

    public Region Offset(Position offset)
    {
        return Offset(offset.X, offset.Y);
    }

    public Region Offset(int offsetX, int offsetY)
    {
        return new Region(
            x: X + offsetX,
            y: Y + offsetY,
            Width, Height
        );
    }

    public Region Union(Region other)
    {
        var x = Math.Min(X, other.X);
        var y = Math.Min(Y, other.Y);
        return new Region(x, y,
            Math.Max(Right, other.Right) - x,
            Math.Max(Bottom, other.Bottom) - y);
    }

    public void Deconstruct(out int x, out int y, out int width, out int height)
    {
        x = X;
        y = Y;
        width = Width;
        height = Height;
    }

    public static bool operator ==(Region left, Region right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Region left, Region right)
    {
        return !(left == right);
    }

    public override string ToString()
    {
        return $"{X},{Y},{Width},{Height}";
    }
}