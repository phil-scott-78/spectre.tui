namespace Spectre.Tui;

public sealed class Buffer
{
    public Region Region { get; private set; }
    public Cell[] Cells { get; private set; }
    public int Length { get; private set; }

    private Buffer(Region region, Cell[] cells)
    {
        Region = region;
        Cells = cells ?? throw new ArgumentNullException(nameof(cells));
        Length = region.Area;

        if (Length != Cells.Length)
        {
            throw new InvalidOperationException("Mismatch between buffer size and provided area");
        }
    }

    public static Buffer Empty(Size size)
    {
        return Empty(new Region(0, 0, size.Width, size.Height));
    }

    public static Buffer Empty(Region region)
    {
        return Filled(region, new Cell());
    }

    public static Buffer Filled(Size size, Cell cell)
    {
        return Filled(new Region(0, 0, size.Width, size.Height), cell);
    }

    public static Buffer Filled(Region area, Cell cell)
    {
        var cells = new Cell[area.Area];
        for (var i = 0; i < cells.Length; i++)
        {
            cells[i] = cell.Clone();
        }

        return new Buffer(area, cells);
    }

    public Cell? GetCell(int index)
    {
        return index < 0 || index >= Length
            ? null
            : Cells[index];
    }

    public Cell? GetCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Region.Width || y >= Region.Height)
        {
            return null;
        }

        var index = (y * Region.Width) + x;
        return index >= Length ? null : Cells[index];
    }

    public void Render<T>(T widget, Region area)
        where T : IWidget
    {
        widget.Render(area, this);
    }

    public void Render<T, TState>(T widget, Region area, TState state)
        where T : IWidget<TState>
    {
        widget.Render(area, this, state);
    }

    public void Reset()
    {
        Filled(Region, new Cell());
    }

    public void Resize(Region area)
    {
        var cells = new Cell[area.Area];
        Filled(Region, new Cell());

        Cells = cells;
        Region = area;
        Length = Region.Area;
    }

    public IEnumerable<(int x, int y, Cell)> Diff(Buffer other)
    {
        foreach (var (index, (current, previous)) in other.Cells.Zip(Cells).Index())
        {
            if (current.Equals(previous))
            {
                continue;
            }

            var x = (index % Region.Width) + Region.X;
            var y = (index / Region.Width) + Region.Y;
            yield return (x, y, current);
        }
    }
}