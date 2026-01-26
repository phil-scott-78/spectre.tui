namespace Spectre.Tui.Ansi;

internal sealed class UnixTerminal(AnsiCapabilities capabilities, ITerminalMode mode)
    : AnsiTerminal(capabilities, mode)
{
    [DllImport("libc")]
    private static extern int write(int fd, byte[] buf, int n);

    public override Size GetSize()
    {
        // TODO: Use ioctl with TIOCGWINSZ
        return Mode.GetSize(System.Console.WindowWidth, System.Console.WindowHeight);
    }

    protected override void Flush(string buffer)
    {
        var bytes = Encoding.UTF8.GetBytes(buffer);
        var _ = write(1, bytes, bytes.Length);
    }
}
