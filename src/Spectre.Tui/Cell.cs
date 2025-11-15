using System.Diagnostics;

namespace Spectre.Tui;

[DebuggerDisplay("{DebuggerDisplay(),nq}")]
public record struct Cell
{
    public Cell()
    {
        Rune = default;
    }

    public Rune Rune { get; set; }
    public Decoration Decoration { get; set; } = Decoration.None;

    private string DebuggerDisplay()
    {
        return ((char)Rune.Value).ToString();
    }
}