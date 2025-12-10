namespace Spectre.Tui;

[DebuggerDisplay("{DebuggerDisplay(),nq}")]
public readonly record struct Cell
{
    public Cell()
    {
        Rune = default;
    }

    public Rune Rune { get; init; }
    public Decoration Decoration { get; init; } = Decoration.None;
    public Color Foreground { get; init; } = Color.Default;
    public Color Background { get; init; } = Color.Default;

    private string DebuggerDisplay()
    {
        return ((char)Rune.Value).ToString();
    }
}