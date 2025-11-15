namespace Spectre.Tui;

public interface IWidget
{
    void Render(IRendererContext context);
}