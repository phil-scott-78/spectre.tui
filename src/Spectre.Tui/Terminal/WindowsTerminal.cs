namespace Spectre.Tui.Ansi;

internal sealed class WindowsTerminal(AnsiCapabilities capabilities)
    : AnsiTerminal(capabilities)
{
    public WindowsTerminal(ColorSystem colors, ITerminalMode mode)
        : base(colors, mode)
    {
    }

    protected override void Flush(string buffer)
    {
        System.Console.Write(buffer);
    }

    public override Size GetSize()
    {
        // TODO: Use Win32 API to get console size
        return new Size(System.Console.WindowWidth, System.Console.WindowHeight);
        return Mode.GetSize(Console.WindowWidth, Console.WindowHeight);
    }
}
