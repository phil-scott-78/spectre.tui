using System.Diagnostics;
using System.Text;

namespace Spectre.Tui;

[DebuggerDisplay("{DebuggerDisplay(),nq}")]
public sealed class Cell : IEquatable<Cell>
{
    public Rune Rune { get; init; } = new Rune(' ');
    public Decoration Decoration { get; init; } = Decoration.None;

    public bool Equals(Cell? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Rune.Equals(other.Rune) &&
               Decoration == other.Decoration;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is Cell other && Equals(other));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Rune, (int)Decoration);
    }

    private string DebuggerDisplay()
    {
        return ((char)Rune.Value).ToString();
    }
}