using System.Text;
using Shouldly;
using Spectre.Tui.Testing;

namespace Spectre.Tui.Tests;

public sealed class RendererTests
{
    private sealed class TestTextWidget(int x, int y, string text) : IWidget
    {
        public void Render(Region area, Buffer buffer)
        {
            for (var i = 0; i < text.Length; i++)
            {
                buffer
                    .GetCell(x + i, y)?
                    .SetRune(new Rune(text[i]));
            }
        }
    }

    [Fact]
    public void Should_Render_Buffer()
    {
        // Given
        var fixture = new TuiFixture(
            new Size(11, 5));

        // When
        var result = fixture.Render(
            new TestTextWidget(5, 2, "1"));

        // Then
        result.ShouldBe(
            """
            ???????????
            ???????????
            ?????1?????
            ???????????
            ???????????
            """);
    }

    [Fact]
    public void Should_Only_Render_Diff_Between_Frames()
    {
        // Given
        var fixture = new TuiFixture(new Size(11, 5));
        fixture.Render(new TestTextWidget(2, 1, "Hello"));

        // When
        var result = fixture.Render(frame =>
        {
            frame.Render(new TestTextWidget(2, 1, "Hello"));
            frame.Render(new TestTextWidget(2, 2, "World"));
        });

        // Then
        result.ShouldBe(
            """
            ???????????
            ???????????
            ??World????
            ???????????
            ???????????
            """);
    }
}