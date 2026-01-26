using Spectre.Tui.Ansi;

namespace Spectre.Tui;

[PublicAPI]
public sealed class InlineMode : ITerminalMode
{
    private int _reservedLines;
    private int _currentHeight;

    public int Height { get; private set; }

    public InlineMode(int height)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);

        Height = height;
        _currentHeight = height;
    }

    public void SetHeight(int height)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        Height = height;
    }

    /// <summary>
    /// Returns the effective size for rendering. Updates <c>_currentHeight</c>
    /// to reflect the clamped height (never exceeds the terminal height).
    /// Called every frame by the renderer.
    /// </summary>
    public Size GetSize(int terminalWidth, int terminalHeight)
    {
        _currentHeight = Math.Min(Height, terminalHeight);
        return new Size(terminalWidth, _currentHeight);
    }

    public void OnAttach(Action<string> write)
    {
        // Reserve scrollback space
        for (var i = 0; i < Height; i++)
        {
            write("\n");
        }

        // Move back up
        write(AnsiSequences.CursorUp(Height));

        // Save cursor position and hide cursor
        write(AnsiSequences.SaveCursor);
        write(AnsiSequences.HideCursor);

        _reservedLines = Height;
    }

    public void OnDetach(Action<string> write)
    {
        // Restore to top of region
        write(AnsiSequences.RestoreCursor);

        // Move past the reserved region
        if (_reservedLines > 0)
        {
            write(AnsiSequences.CursorDown(_reservedLines));
        }

        // Show cursor and add newline
        write(AnsiSequences.ShowCursor);
        write("\n");
    }

    public void Clear(Action<string> write)
    {
        if (_currentHeight > _reservedLines)
        {
            // Grow: reserve additional scrollback lines
            write(AnsiSequences.RestoreCursor);
            if (_reservedLines > 0)
            {
                write(AnsiSequences.CursorDown(_reservedLines));
            }

            var additional = _currentHeight - _reservedLines;
            for (var i = 0; i < additional; i++)
            {
                write("\n");
            }

            write(AnsiSequences.CursorUp(_currentHeight));
            write(AnsiSequences.SaveCursor);
        }

        // Clear all lines (covers current region + any excess from shrinking)
        var linesToClear = Math.Max(_currentHeight, _reservedLines);
        write(AnsiSequences.RestoreCursor);
        for (var i = 0; i < linesToClear; i++)
        {
            if (i > 0)
            {
                write(AnsiSequences.CursorDown(1));
            }

            write(AnsiSequences.EraseLine);
        }

        write(AnsiSequences.RestoreCursor);

        _reservedLines = _currentHeight;
    }

    public void MoveTo(int x, int y, Action<string> write)
    {
        // Restore to saved position (top-left of region)
        write(AnsiSequences.RestoreCursor);

        // Move down if needed
        if (y > 0)
        {
            write(AnsiSequences.CursorDown(y));
        }

        // Move to absolute column
        write(AnsiSequences.CursorToColumn(x + 1));
    }
}
