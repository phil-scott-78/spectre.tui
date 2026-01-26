namespace Spectre.Tui;

[PublicAPI]
public interface ITerminalMode
{
    Size GetSize(int terminalWidth, int terminalHeight);
    void OnAttach(AnsiWriter writer);
    void OnDetach(AnsiWriter writer);
    void Clear(AnsiWriter writer);
    void MoveTo(int x, int y, AnsiWriter writer);
}
