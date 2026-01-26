namespace Spectre.Tui.Tests;

public sealed class FullscreenModeTests
{
    public sealed class TheGetSizeMethod
    {
        [Fact]
        public void Should_Return_Full_Terminal_Dimensions()
        {
            // Given
            var mode = new FullscreenMode();

            // When
            var size = mode.GetSize(80, 24);

            // Then
            size.Width.ShouldBe(80);
            size.Height.ShouldBe(24);
        }
    }

    public sealed class TheOnAttachMethod
    {
        [Fact]
        public void Should_Enable_Alt_Screen_And_Hide_Cursor()
        {
            // Given
            var mode = new FullscreenMode();
            var output = new List<string>();

            // When
            mode.OnAttach(output.Add);

            // Then
            output.ShouldBe([
                "\e[?1049h\e[H",
                "\e[?25l",
            ]);
        }
    }

    public sealed class TheOnDetachMethod
    {
        [Fact]
        public void Should_Disable_Alt_Screen_And_Show_Cursor()
        {
            // Given
            var mode = new FullscreenMode();
            var output = new List<string>();

            // When
            mode.OnDetach(output.Add);

            // Then
            output.ShouldBe([
                "\e[?1049l",
                "\e[?25h",
            ]);
        }
    }

    public sealed class TheClearMethod
    {
        [Fact]
        public void Should_Erase_Display()
        {
            // Given
            var mode = new FullscreenMode();
            var output = new List<string>();

            // When
            mode.Clear(output.Add);

            // Then
            output.ShouldBe([
                "\e[2J",
            ]);
        }
    }

    public sealed class TheMoveToMethod
    {
        [Fact]
        public void Should_Emit_One_Based_Cursor_Position()
        {
            // Given
            var mode = new FullscreenMode();
            var output = new List<string>();

            // When
            mode.MoveTo(0, 0, output.Add);

            // Then
            output.ShouldBe([
                "\e[1;1H",
            ]);
        }

        [Fact]
        public void Should_Offset_Coordinates_By_One()
        {
            // Given
            var mode = new FullscreenMode();
            var output = new List<string>();

            // When
            mode.MoveTo(9, 4, output.Add);

            // Then
            output.ShouldBe([
                "\e[5;10H",
            ]);
        }
    }
}
