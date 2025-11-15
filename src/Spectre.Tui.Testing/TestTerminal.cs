using System.Text;

namespace Spectre.Tui.Testing;

public sealed class TestTerminal : ITerminal
{
    private readonly Size _size;
    private readonly char[] _buffer;

    public string Output { get; private set; } = "[Terminal buffer not flushed]";

    public TestTerminal(Size? size)
    {
        _size = size ?? new Size(80, 25);
        _buffer = new char[_size.Area];

        Array.Fill(_buffer, ' ');
    }

    public void Dispose()
    {
    }

    public void Clear()
    {
        Array.Fill(_buffer, ' ');
    }

    public Size GetSize()
    {
        return _size;
    }

    public void Write(IEnumerable<(int x, int y, Cell cell)> updates)
    {
        var items = updates.ToArray();

        // Write updates
        foreach (var (x, y, cell) in items)
        {
            var index = (y * _size.Width) + x;
            _buffer[index] = (char)cell.Rune.Value;
        }

        // Write empty spaces (to make it easier to read)
        for (var y = 0; y < _size.Height; y++)
        {
            for (var x = 0; x < _size.Width; x++)
            {
                if (!items.Any(item => item.x == x && item.y == y))
                {
                    _buffer[(y * _size.Width) + x] = '?';
                }
            }
        }
    }

    public void Flush()
    {
        Output = Render();
    }

    private string Render()
    {
        var output = new StringBuilder();

        for (var y = 0; y < _size.Height; y++)
        {
            for (var x = 0; x < _size.Width; x++)
            {
                output.Append(_buffer[(y * _size.Width) + x]);
            }

            if (y != _size.Height - 1)
            {
                output.AppendLine();
            }
        }

        return output.ToString();
    }
}