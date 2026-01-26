namespace Spectre.Tui.Tests;

public sealed class InlineModeTests
{
    public sealed class TheGetSizeMethod
    {
        [Fact]
        public void Should_Return_Full_Width_And_Requested_Height()
        {
            // Given
            var mode = new InlineMode(5);

            // When
            var size = mode.GetSize(80, 24);

            // Then
            size.Width.ShouldBe(80);
            size.Height.ShouldBe(5);
        }

        [Fact]
        public void Should_Clamp_Height_To_Terminal_Height()
        {
            // Given
            var mode = new InlineMode(30);

            // When
            var size = mode.GetSize(80, 24);

            // Then
            size.Width.ShouldBe(80);
            size.Height.ShouldBe(24);
        }

        [Fact]
        public void Should_Reflect_Height_Change_Via_SetHeight()
        {
            // Given
            var mode = new InlineMode(5);
            mode.SetHeight(10);

            // When
            var size = mode.GetSize(80, 24);

            // Then
            size.Width.ShouldBe(80);
            size.Height.ShouldBe(10);
        }
    }

    public sealed class TheOnAttachMethod
    {
        [Fact]
        public void Should_Reserve_Scrollback_Lines()
        {
            // Given
            var mode = new InlineMode(3);
            var output = new List<string>();

            // When
            mode.OnAttach(output.Add);

            // Then
            output.ShouldBe([
                "\n", "\n", "\n",
                "\e[3A",
                "\e[s",
                "\e[?25l",
            ]);
        }
    }

    public sealed class TheOnDetachMethod
    {
        [Fact]
        public void Should_Restore_Cursor_Move_Past_Region_And_Show_Cursor()
        {
            // Given
            var mode = new InlineMode(3);
            mode.OnAttach(_ => { });
            var output = new List<string>();

            // When
            mode.OnDetach(output.Add);

            // Then
            output.ShouldBe([
                "\e[u",
                "\e[3B",
                "\e[?25h",
                "\n",
            ]);
        }

        [Fact]
        public void Should_Skip_CursorDown_When_No_Reserved_Lines()
        {
            // Given
            var mode = new InlineMode(3);
            var output = new List<string>();

            // When
            mode.OnDetach(output.Add);

            // Then
            output.ShouldBe([
                "\e[u",
                "\e[?25h",
                "\n",
            ]);
        }
    }

    public sealed class TheClearMethod
    {
        [Fact]
        public void Should_Erase_Each_Line_In_Steady_State()
        {
            // Given
            var mode = new InlineMode(3);
            mode.OnAttach(_ => { });
            mode.GetSize(80, 24);
            var output = new List<string>();

            // When
            mode.Clear(output.Add);

            // Then
            output.ShouldBe([
                "\e[u",
                "\e[2K",
                "\e[1B", "\e[2K",
                "\e[1B", "\e[2K",
                "\e[u",
            ]);
        }

        [Fact]
        public void Should_Reserve_Additional_Lines_When_Growing()
        {
            // Given
            var mode = new InlineMode(3);
            mode.OnAttach(_ => { });
            mode.SetHeight(5);
            mode.GetSize(80, 24);
            var output = new List<string>();

            // When
            mode.Clear(output.Add);

            // Then
            output.ShouldBe([
                // Grow: restore, move past reserved, add newlines, move back up, save
                "\e[u",
                "\e[3B",
                "\n", "\n",
                "\e[5A",
                "\e[s",
                // Clear 5 lines
                "\e[u",
                "\e[2K",
                "\e[1B", "\e[2K",
                "\e[1B", "\e[2K",
                "\e[1B", "\e[2K",
                "\e[1B", "\e[2K",
                "\e[u",
            ]);
        }

        [Fact]
        public void Should_Clear_Old_Reserved_Region_When_Shrinking()
        {
            // Given
            var mode = new InlineMode(5);
            mode.OnAttach(_ => { });
            mode.SetHeight(3);
            mode.GetSize(80, 24);
            var output = new List<string>();

            // When
            mode.Clear(output.Add);

            // Then
            output.ShouldBe([
                // No grow block
                // Clear max(3, 5) = 5 lines
                "\e[u",
                "\e[2K",
                "\e[1B", "\e[2K",
                "\e[1B", "\e[2K",
                "\e[1B", "\e[2K",
                "\e[1B", "\e[2K",
                "\e[u",
            ]);
        }

        [Fact]
        public void Should_Handle_Grow_From_Zero_Reserved_Lines()
        {
            // Given
            var mode = new InlineMode(3);
            // No OnAttach, so _reservedLines = 0
            var output = new List<string>();

            // When
            mode.Clear(output.Add);

            // Then
            output.ShouldBe([
                // Grow: restore, skip CursorDown (0 reserved), add 3 newlines, move up, save
                "\e[u",
                "\n", "\n", "\n",
                "\e[3A",
                "\e[s",
                // Clear 3 lines
                "\e[u",
                "\e[2K",
                "\e[1B", "\e[2K",
                "\e[1B", "\e[2K",
                "\e[u",
            ]);
        }
    }

    public sealed class TheMoveToMethod
    {
        [Fact]
        public void Should_Move_To_Origin()
        {
            // Given
            var mode = new InlineMode(5);
            var output = new List<string>();

            // When
            mode.MoveTo(0, 0, output.Add);

            // Then
            output.ShouldBe([
                "\e[u",
                "\e[1G",
            ]);
        }

        [Fact]
        public void Should_Move_Down_And_To_Column()
        {
            // Given
            var mode = new InlineMode(5);
            var output = new List<string>();

            // When
            mode.MoveTo(5, 3, output.Add);

            // Then
            output.ShouldBe([
                "\e[u",
                "\e[3B",
                "\e[6G",
            ]);
        }
    }

    public sealed class Lifecycle
    {
        [Fact]
        public void Should_Produce_Correct_Sequence_For_Full_Lifecycle()
        {
            // Given
            var mode = new InlineMode(3);
            var output = new List<string>();

            // When
            mode.OnAttach(output.Add);
            mode.GetSize(80, 24);
            mode.Clear(output.Add);
            mode.MoveTo(2, 1, output.Add);
            mode.OnDetach(output.Add);

            // Then
            output.ShouldBe([
                // OnAttach: reserve 3 lines
                "\n", "\n", "\n",
                "\e[3A",
                "\e[s",
                "\e[?25l",
                // Clear: steady state (3 == 3)
                "\e[u",
                "\e[2K",
                "\e[1B", "\e[2K",
                "\e[1B", "\e[2K",
                "\e[u",
                // MoveTo(2, 1)
                "\e[u",
                "\e[1B",
                "\e[3G",
                // OnDetach
                "\e[u",
                "\e[3B",
                "\e[?25h",
                "\n",
            ]);
        }
    }
}
