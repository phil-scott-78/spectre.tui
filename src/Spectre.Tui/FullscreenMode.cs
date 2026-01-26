using Spectre.Tui.Ansi;

namespace Spectre.Tui;

[PublicAPI]
public sealed class FullscreenMode : ITerminalMode
{
    public Size GetSize(int terminalWidth, int terminalHeight)
    {
        return new Size(terminalWidth, terminalHeight);
    }

    public void OnAttach(Action<string> write)
    {
        write(AnsiSequences.EnableAltScreen + AnsiSequences.CursorHome);
        write(AnsiSequences.HideCursor);
    }

    public void OnDetach(Action<string> write)
    {
        write(AnsiSequences.DisableAltScreen);
        write(AnsiSequences.ShowCursor);
    }

    public void Clear(Action<string> write)
    {
        write(AnsiSequences.EraseDisplay);
    }

    public void MoveTo(int x, int y, Action<string> write)
    {
        write(AnsiSequences.CursorPosition(y + 1, x + 1));
    }
}
