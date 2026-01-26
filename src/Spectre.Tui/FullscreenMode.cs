namespace Spectre.Tui;

[PublicAPI]
public sealed class FullscreenMode : ITerminalMode
{
    public Size GetSize(int terminalWidth, int terminalHeight)
    {
        return new Size(terminalWidth, terminalHeight);
    }

    public void OnAttach(AnsiWriter writer)
    {
        writer.EnterAltScreen();
        writer.CursorHome();
        writer.HideCursor();
    }

    public void OnDetach(AnsiWriter writer)
    {
        writer.ExitAltScreen();
        writer.ShowCursor();
    }

    public void Clear(AnsiWriter writer)
    {
        writer.EraseInDisplay(2);
    }

    public void MoveTo(int x, int y, AnsiWriter writer)
    {
        writer.CursorPosition(y + 1, x + 1);
    }
}
