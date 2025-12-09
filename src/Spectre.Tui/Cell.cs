using System.Diagnostics;

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

    private string DebuggerDisplay()
    {
        return ((char)Rune.Value).ToString();
    }
}