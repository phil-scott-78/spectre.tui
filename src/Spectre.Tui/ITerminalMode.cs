namespace Spectre.Tui;

[PublicAPI]
public interface ITerminalMode
{
    Size GetSize(int terminalWidth, int terminalHeight);
    void OnAttach(Action<string> write);
    void OnDetach(Action<string> write);
    void Clear(Action<string> write);
    void MoveTo(int x, int y, Action<string> write);
}
