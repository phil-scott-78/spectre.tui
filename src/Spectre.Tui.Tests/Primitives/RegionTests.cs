using Shouldly;

namespace Spectre.Tui.Tests;

public sealed class RegionTests
{
    [Fact]
    public void Should_Assign_Properties_Correctly()
    {
        // Given, When
        var rect = new Region(1, 2, 3, 4);

        // Then
        rect.X.ShouldBe(1);
        rect.Y.ShouldBe(2);
        rect.Width.ShouldBe(3);
        rect.Height.ShouldBe(4);
        rect.Top.ShouldBe(2);
        rect.Bottom.ShouldBe(6);
        rect.Left.ShouldBe(1);
        rect.Right.ShouldBe(4);
    }

    [Fact]
    public void Should_Deconstruct_As_Expected()
    {
        // Given
        var region = new Region(1, 2, 5, 10);

        // When
        var (x, y, width, height) = region;

        // Then
        x.ShouldBe(1);
        y.ShouldBe(2);
        width.ShouldBe(5);
        height.ShouldBe(10);
    }

    public sealed class TheInflateMethod
    {
        [Fact]
        public void Should_Inflate_Region_With_Expected_Size()
        {
            // Given
            var region = new Region(1, 2, 5, 10);

            // When
            var result = region.Inflate(2, 3);

            // Then
            result.ShouldBe(
                new Region(-1, -1, 9, 16));
        }

        [Fact]
        public void Should_Deflate_Region_With_Expected_Size_If_Size_Is_Negative()
        {
            // Given
            var region = new Region(1, 2, 5, 10);

            // When
            var result = region.Inflate(-1, -2);

            // Then
            result.ShouldBe(
                new Region(2, 4, 3, 6));
        }
    }

    public sealed class TheOffsetMethod
    {
        [Fact]
        public void Should_Offset_Region()
        {
            // Given
            var region = new Region(1, 2, 5, 10);

            // When
            var result = region.Offset(2, 3);

            // Then
            result.ShouldBe(
                new Region(3, 5, 5, 10));
        }
    }

    public sealed class TheUnionMethod
    {
        [Fact]
        public void Should_Offset_Region()
        {
            // Given
            var region = new Region(1, 2, 5, 10);
            var other = new Region(3, 4, 15, 20);

            // When
            var result = region.Union(ref other);

            // Then
            result.ShouldBe(
                new Region(1, 2, 17, 22));
        }
    }
}