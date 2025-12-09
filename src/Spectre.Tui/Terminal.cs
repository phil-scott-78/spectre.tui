using System.Runtime.InteropServices;

namespace Spectre.Tui;

public static class Terminal
{
    public static ITerminal Create()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new WindowsTerminal();
        }

        return new UnixTerminal();
    }
}
