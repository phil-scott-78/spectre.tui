namespace Spectre.Tui;

internal sealed class Buffer
{
    public Rectangle Screen { get; private set; }
    public Cell[] Cells { get; private set; }
    public int Length { get; private set; }

    internal Buffer(Rectangle screen, Cell[] cells)
    {
        Screen = screen;
        Cells = cells ?? throw new ArgumentNullException(nameof(cells));
        Length = screen.CalculateArea();

        if (Length != Cells.Length)
        {
            throw new InvalidOperationException("Mismatch between buffer size and provided area");
        }
    }

    public Cell GetCell(int index)
    {
        return index < 0 || index >= Length
            ? default
            : Cells[index];
    }

    public Cell GetCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Screen.Width || y >= Screen.Height)
        {
            return default;
        }

        return Cells[(y * Screen.Width) + x];
    }

    public void SetRune(int x, int y, Rune rune)
    {
        TrySetRune(x, y, rune, out _);
    }

    private bool TrySetRune(int x, int y, Rune rune, out bool moveForward)
    {
        var index = (y * Screen.Width) + x;
        if (index < 0 || index >= Cells.Length)
        {
            moveForward = false;
            return false;
        }

        Cells[index].Rune = rune;
        moveForward = true;
        return true;
    }

    public void Reset()
    {
        var cells = new Cell[Screen.CalculateArea()];
        Array.Fill(cells, new Cell());
        Cells = cells;
    }

    public void Resize(Rectangle area)
    {
        var cells = new Cell[area.CalculateArea()];
        Array.Fill(cells, new Cell());

        Cells = cells;
        Screen = area;
        Length = Screen.CalculateArea();
    }

    public IEnumerable<(int x, int y, Cell)> Diff(Buffer other)
    {
        foreach (var (index, (current, previous)) in other.Cells.Zip(Cells).Index())
        {
            if (current.Equals(previous) || current == default)
            {
                continue;
            }

            var x = (index % Screen.Width) + Screen.X;
            var y = (index / Screen.Width) + Screen.Y;
            yield return (x, y, current);
        }
    }
}

internal static class BufferExtensions
{
    extension(Buffer)
    {
        public static Buffer Empty(Rectangle region)
        {
            return Filled(region, new Cell());
        }

        public static Buffer Filled(Rectangle area, Cell cell)
        {
            var cells = new Cell[area.CalculateArea()];
            Array.Fill(cells, cell);
            return new Buffer(area, cells);
        }
    }
}