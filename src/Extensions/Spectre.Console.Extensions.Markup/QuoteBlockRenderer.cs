using Markdig.Syntax;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup;

internal class QuoteBlockRenderer : IRenderer<QuoteBlock>
{
    private readonly MarkdownInlineRendering _inlineRendering = new();

    public IRenderable Render(QuoteBlock quoteBlock, BlockRenderer blockRenderer)
    {
        var renderables = new CompositeRenderable(quoteBlock.Select(x =>
        {
            if (x is ParagraphBlock { Inline: not null } paragraphBlock)
            {
                return _inlineRendering.RenderInline(paragraphBlock.Inline, Style.Plain, Justify.Left);
            }
            else
            {
                return blockRenderer.Render(x);
            }
        }));

        var panel = new Panel(renderables)
        {
            Border = new LeftBorder(),
            BorderStyle = new Style(Color.Green),
            Padding = new Padding(1, 1)
        };

        return panel;
    }
}
