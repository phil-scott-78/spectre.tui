namespace Spectre.Tui;

internal abstract class AnsiTerminal : ITerminal
{
    private readonly StringBuilder _buffer;

    public bool SupportsAnsi { get; protected set; } = true;

    protected AnsiTerminal()
    {
        _buffer = new StringBuilder();

        Write("\e[?1049h\e[H");
        Write("\e[?25l");
        Flush();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Write("\e[?1049l");
            Write("\e[?25h");
            Flush();
        }
    }

    public void Flush()
    {
        try
        {
            Flush(_buffer.ToString());
        }
        finally
        {
            _buffer.Clear();
        }
    }

    protected abstract void Flush(string buffer);

    public void Clear()
    {
        Write("\e[2J");
    }

    public abstract Size GetSize();

    public void MoveTo(int x, int y)
    {
        Write($"\e[{y + 1};{x + 1}H");
    }

    public void Write(Cell cell)
    {
        Write(AnsiBuilder.GetAnsi(ref cell, ColorSystem.TrueColor));
    }

    private void Write(ReadOnlySpan<char> text)
    {
        _buffer.Append(text.ToArray());
    }
}