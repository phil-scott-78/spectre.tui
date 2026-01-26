namespace Spectre.Tui.Ansi;

internal sealed class WindowsTerminal(AnsiCapabilities capabilities, ITerminalMode mode)
    : AnsiTerminal(capabilities, mode)
{
    protected override void Flush(string buffer)
    {
        System.Console.Write(buffer);
    }

    public override Size GetSize()
    {
        // TODO: Use Win32 API to get console size
        return Mode.GetSize(System.Console.WindowWidth, System.Console.WindowHeight);
    }
}
