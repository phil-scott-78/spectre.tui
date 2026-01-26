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
            var fixture = new AnsiWriterFixture();
            var mode = new FullscreenMode();

            // When
            mode.OnAttach(fixture.Writer);

            // Then
            fixture.Output.ShouldBe(
                "\e[?1049h\e[H" +
                "\e[?25l"
            );
        }
    }

    public sealed class TheOnDetachMethod
    {
        [Fact]
        public void Should_Disable_Alt_Screen_And_Show_Cursor()
        {
            // Given
            var fixture = new AnsiWriterFixture();
            var mode = new FullscreenMode();

            // When
            mode.OnDetach(fixture.Writer);

            // Then
            fixture.Output.ShouldBe(
                "\e[?1049l" +
                "\e[?25h"
            );
        }
    }

    public sealed class TheClearMethod
    {
        [Fact]
        public void Should_Erase_Display()
        {
            // Given
            var fixture = new AnsiWriterFixture();
            var mode = new FullscreenMode();

            // When
            mode.Clear(fixture.Writer);

            // Then
            fixture.Output.ShouldBe("\e[2J");
        }
    }

    public sealed class TheMoveToMethod
    {
        [Fact]
        public void Should_Emit_One_Based_Cursor_Position()
        {
            // Given
            var fixture = new AnsiWriterFixture();
            var mode = new FullscreenMode();

            // When
            mode.MoveTo(0, 0, fixture.Writer);

            // Then
            fixture.Output.ShouldBe("\e[1;1H");
        }

        [Fact]
        public void Should_Offset_Coordinates_By_One()
        {
            // Given
            var fixture = new AnsiWriterFixture();
            var mode = new FullscreenMode();

            // When
            mode.MoveTo(9, 4, fixture.Writer);

            // Then
            fixture.Output.ShouldBe("\e[5;10H");
        }
    }
}
