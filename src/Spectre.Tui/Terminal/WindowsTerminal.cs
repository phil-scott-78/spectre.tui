namespace Spectre.Tui;

internal sealed class WindowsTerminal : AnsiTerminal
{
    protected override void Flush(string buffer)
    {
        Console.Write(buffer);
    }

    public override Size GetSize()
    {
        // TODO: Use Win32 API to get console size
        return new Size(Console.WindowWidth, Console.WindowHeight);
    }
}