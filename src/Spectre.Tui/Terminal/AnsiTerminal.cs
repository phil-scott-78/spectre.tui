namespace Spectre.Tui.Ansi;

[PublicAPI]
public abstract class AnsiTerminal : ITerminal
{
    private readonly StringBuilder _buffer;
    private readonly AnsiWriter _writer;
    private readonly AnsiState _state;
    private readonly AnsiBuilder _ansi;
    private readonly Action<string> _write;

    public AnsiCapabilities Capabilities { get; }
    public ColorSystem ColorSystem { get; protected set; }
    protected ITerminalMode Mode { get; }

    protected AnsiTerminal(AnsiCapabilities capabilities)
    protected AnsiTerminal(ColorSystem colors, ITerminalMode mode)
    {
        Capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));

        _buffer = new StringBuilder();
        _writer = new AnsiWriter(new StringWriter(_buffer), capabilities);
        _state = new AnsiState(_writer);
        _ansi = new AnsiBuilder();
        _write = text => _buffer.Append(text);

        _writer
            .EnterAltScreen()
            .CursorHome()
            .HideCursor();
        ColorSystem = colors;
        Mode = mode ?? throw new ArgumentNullException(nameof(mode));

        Mode.OnAttach(_write);
        Flush();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected abstract void Flush(string buffer);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _writer
                .ExitAltScreen()
                .ShowCursor();

            Mode.OnDetach(_write);
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

    public void Clear()
    {
        _writer.EraseInDisplay(2);
        Mode.Clear(_write);
    }

    public abstract Size GetSize();

    public void MoveTo(int x, int y)
    {
        _writer.CursorPosition(y + 1, x + 1);
        Mode.MoveTo(x, y, _write);
    }

    public void Write(Cell cell)
    {
        if (!_state.Update(cell))
        {
            // State did not change
            _writer.Write(cell.Symbol);
            return;
        }

        // Reset SGR attributes
        _writer.ResetStyle();

        // Write the cell appearance
        _state.Write();

        // Write the cell symbol
        _writer.Write(cell.Symbol);

        // Swap the states
        _state.Swap();
    }

    private sealed class AnsiState(AnsiWriter writer)
    {
        private Appearance? _current;
        private Appearance? _previous;
        private readonly AnsiWriter _writer = writer;

        public Appearance? Current => _current;

        public bool Update(Cell cell)
        {
            _current = cell.Style;

            // First time we run?
            if (_previous == null)
            {
                return true;
            }

            return Current != _previous;
        }

        public void Swap()
        {
            _previous = Current;
            _current = null;
        }

        public void Write()
        {
            if (!Current.HasValue)
            {
                throw new InvalidOperationException("State has not been updated");
            }

            // Decoration
            _writer.Decoration(Current.Value.Decoration);

            // Foreground
            if (Current.Value.Foreground != Color.Default)
            {
                _writer.Foreground(Current.Value.Foreground);
            }

            // Background
            if (Current.Value.Background != Color.Default)
            {
                _writer.Background(Current.Value.Foreground);
            }
        }
    }
}
