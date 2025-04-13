using System.Text;
using Markdig.Syntax;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup;

internal class LeafBlockRenderer : IRenderer<LeafBlock>
{
    private readonly MarkdownInlineRendering _inlineRendering = new();

    public IRenderable Render(LeafBlock textBlock, BlockRenderer blockRenderer)
    {
        if (textBlock.Inline is null)
        {
            return Text.Empty;
        }

        var r = _inlineRendering.RenderContainerInline(textBlock.Inline, Style.Plain);

        var grid = new Grid { Width = 120, }
            .AddColumn()
            .AddRow(r);

        return grid;
    }
}