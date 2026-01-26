namespace Spectre.Tui;

internal static class AnsiWriterExtensions
{
    public static void SaveCursor(this AnsiWriter writer)
    {
        writer.Write("\e[s");
    }

    public static void RestoreCursor(this AnsiWriter writer)
    {
        writer.Write("\e[u");
    }

    public static void CursorToColumn(this AnsiWriter writer, int column)
    {
        writer.Write($"\e[{column}G");
    }
}
