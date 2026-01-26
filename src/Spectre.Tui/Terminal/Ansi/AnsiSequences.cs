namespace Spectre.Tui.Ansi;

internal static class AnsiSequences
{
    public const string SaveCursor = "\e[s";
    public const string RestoreCursor = "\e[u";
    public const string HideCursor = "\e[?25l";
    public const string ShowCursor = "\e[?25h";
    public const string EraseLine = "\e[2K";
    public const string EraseDisplay = "\e[2J";
    public const string CursorHome = "\e[H";
    public const string EnableAltScreen = "\e[?1049h";
    public const string DisableAltScreen = "\e[?1049l";

    public static string CursorUp(int n) => $"\e[{n}A";
    public static string CursorDown(int n) => $"\e[{n}B";
    public static string CursorToColumn(int col) => $"\e[{col}G";
    public static string CursorPosition(int row, int col) => $"\e[{row};{col}H";
}
