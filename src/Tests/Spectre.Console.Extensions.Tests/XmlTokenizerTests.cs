using Spectre.Console.Xml;

namespace Spectre.Console.Extensions.Tests;

public class XmlTokenizerTests
{
    [Fact]
    public void ParseText_WithSimpleElement_ReturnsCorrectTokens()
    {
        // Arrange
        var xml = "<element>text</element>";
        var expected = new[]
        {
            new XmlToken(new TextSpan(0, 1), XmlSyntaxKind.OpeningAngleBracket),
            new XmlToken(new TextSpan(1, 7), XmlSyntaxKind.ElementName),
            new XmlToken(new TextSpan(8, 1), XmlSyntaxKind.ClosingAngleBracket),
            new XmlToken(new TextSpan(9, 4), XmlSyntaxKind.Text),
            new XmlToken(new TextSpan(13, 1), XmlSyntaxKind.OpeningAngleBracket),
            new XmlToken(new TextSpan(15, 7), XmlSyntaxKind.EndElement),
            new XmlToken(new TextSpan(22, 1), XmlSyntaxKind.ClosingAngleBracket)
        };

        // Act
        var result = XmlTokenizer.ParseText(xml);

        // Assert
        Assert.Equal(expected.Length, result.Count);
        for (var i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i].Span, result[i].Span);
            Assert.Equal(expected[i].Kind, result[i].Kind);
        }
    }

    [Fact]
    public void ParseText_WithAttribute_ReturnsCorrectTokens()
    {
        // Arrange
        var xml = "<element attr=\"value\">text</element>";
        var expected = new[]
        {
            new XmlToken(new TextSpan(0, 1), XmlSyntaxKind.OpeningAngleBracket),
            new XmlToken(new TextSpan(1, 7), XmlSyntaxKind.ElementName),
            new XmlToken(new TextSpan(8, 1), XmlSyntaxKind.Whitespace),
            new XmlToken(new TextSpan(9, 4), XmlSyntaxKind.AttributeName),
            new XmlToken(new TextSpan(13, 1), XmlSyntaxKind.AttributeEquals),
            new XmlToken(new TextSpan(14, 1), XmlSyntaxKind.AttributeQuote),
            new XmlToken(new TextSpan(15, 5), XmlSyntaxKind.AttributeValue),
            new XmlToken(new TextSpan(20, 1), XmlSyntaxKind.AttributeQuote),
            new XmlToken(new TextSpan(21, 1), XmlSyntaxKind.ClosingAngleBracket),
            new XmlToken(new TextSpan(22, 4), XmlSyntaxKind.Text),
            new XmlToken(new TextSpan(26, 1), XmlSyntaxKind.OpeningAngleBracket),
            new XmlToken(new TextSpan(28, 7), XmlSyntaxKind.EndElement),
            new XmlToken(new TextSpan(35, 1), XmlSyntaxKind.ClosingAngleBracket)
        };

        // Act
        var result = XmlTokenizer.ParseText(xml);

        // Assert
        Assert.Equal(expected.Length, result.Count);
        for (var i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i].Span, result[i].Span);
            Assert.Equal(expected[i].Kind, result[i].Kind);
        }
    }

    [Fact]
    public void ParseText_WithSelfClosingElement_ReturnsCorrectTokens()
    {
        // Arrange
        var xml = "<element/>";
        var expected = new[]
        {
            new XmlToken(new TextSpan(0, 1), XmlSyntaxKind.OpeningAngleBracket),
            new XmlToken(new TextSpan(1, 7), XmlSyntaxKind.ElementName),
            new XmlToken(new TextSpan(8, 1), XmlSyntaxKind.SelfClosingSlash),
            new XmlToken(new TextSpan(9, 1), XmlSyntaxKind.ClosingAngleBracket)
        };

        // Act
        var result = XmlTokenizer.ParseText(xml);

        // Assert
        Assert.Equal(expected.Length, result.Count);
        for (var i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i].Span, result[i].Span);
            Assert.Equal(expected[i].Kind, result[i].Kind);
        }
    }

    [Fact]
    public void ParseText_WithComment_ReturnsCorrectTokens()
    {
        // Arrange
        var xml = "<!-- comment -->";
        var expected = new[]
        {
            new XmlToken(new TextSpan(0, 16), XmlSyntaxKind.Comment)
        };

        // Act
        var result = XmlTokenizer.ParseText(xml);

        // Assert
        Assert.Equal(expected.Length, result.Count);
        Assert.Equal(expected[0].Span, result[0].Span);
        Assert.Equal(expected[0].Kind, result[0].Kind);
    }

    [Fact]
    public void ParseText_WithCData_ReturnsCorrectTokens()
    {
        // Arrange
        var xml = "<![CDATA[<data>]]>";
        var expected = new[]
        {
            new XmlToken(new TextSpan(0, 18), XmlSyntaxKind.CData)
        };

        // Act
        var result = XmlTokenizer.ParseText(xml);

        // Assert
        Assert.Equal(expected.Length, result.Count);
        Assert.Equal(expected[0].Span, result[0].Span);
        Assert.Equal(expected[0].Kind, result[0].Kind);
    }

    [Fact]
    public void ParseText_WithProcessingInstruction_ReturnsCorrectTokens()
    {
        // Arrange
        var xml = "<?xml version=\"1.0\"?>";
        var expected = new[]
        {
            new XmlToken(new TextSpan(0, 21), XmlSyntaxKind.ProcessingInstruction)
        };

        // Act
        var result = XmlTokenizer.ParseText(xml);

        // Assert
        Assert.Equal(expected.Length, result.Count);
        Assert.Equal(expected[0].Span, result[0].Span);
        Assert.Equal(expected[0].Kind, result[0].Kind);
    }

    [Fact]
    public void ParseText_WithDoctype_ReturnsCorrectTokens()
    {
        // Arrange
        var xml = "<!DOCTYPE html>";
        var expected = new[]
        {
            new XmlToken(new TextSpan(0, 15), XmlSyntaxKind.DocumentTypeDeclaration)
        };

        // Act
        var result = XmlTokenizer.ParseText(xml);

        // Assert
        Assert.Equal(expected.Length, result.Count);
        Assert.Equal(expected[0].Span, result[0].Span);
        Assert.Equal(expected[0].Kind, result[0].Kind);
    }

    [Fact]
    public void ParseText_WithWhitespace_ReturnsCorrectTokens()
    {
        // Arrange
        var xml = "   <element>   ";
        var expected = new[]
        {
            new XmlToken(new TextSpan(0, 3), XmlSyntaxKind.Whitespace),
            new XmlToken(new TextSpan(3, 1), XmlSyntaxKind.OpeningAngleBracket),
            new XmlToken(new TextSpan(4, 7), XmlSyntaxKind.ElementName),
            new XmlToken(new TextSpan(11, 1), XmlSyntaxKind.ClosingAngleBracket),
            new XmlToken(new TextSpan(12, 3), XmlSyntaxKind.Whitespace)
        };

        // Act
        var result = XmlTokenizer.ParseText(xml);

        // Assert
        Assert.Equal(expected.Length, result.Count);
        for (var i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i].Span, result[i].Span);
            Assert.Equal(expected[i].Kind, result[i].Kind);
        }
    }

    [Fact]
    public void ParseText_WithComplexXml_ReturnsCorrectTokens()
    {
        // Arrange
        var xml = @"<!DOCTYPE glossary PUBLIC ""-//OASIS//DTD DocBook V3.1//EN"">
<glossary><title>example glossary</title>
  <GlossDiv><title>S</title>
   <GlossList>
    <GlossEntry ID=""SGML"" SortAs=""SGML"">
     <GlossTerm>Standard Generalized Markup Language</GlossTerm>
     <Acronym>SGML</Acronym>
     <Abbrev>ISO 8879:1986</Abbrev>
     <GlossDef>
      <para>A meta-markup language, used to create markup
languages such as DocBook.</para>
      <GlossSeeAlso OtherTerm=""GML"">
      <GlossSeeAlso OtherTerm=""XML"">
     </GlossDef>
     <GlossSee OtherTerm=""markup"">
    </GlossEntry>
   </GlossList>
  </GlossDiv>
 </glossary>";

        // Act
        var result = XmlTokenizer.ParseText(xml);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count > 0);

        // Check that we have the expected token kinds
        var kinds = result.Select(t => t.Kind).ToList();
        Assert.Contains(XmlSyntaxKind.DocumentTypeDeclaration, kinds);
        Assert.Contains(XmlSyntaxKind.OpeningAngleBracket, kinds);
        Assert.Contains(XmlSyntaxKind.ClosingAngleBracket, kinds);
        Assert.Contains(XmlSyntaxKind.ElementName, kinds);
        Assert.Contains(XmlSyntaxKind.AttributeName, kinds);
        Assert.Contains(XmlSyntaxKind.AttributeValue, kinds);
        Assert.Contains(XmlSyntaxKind.Text, kinds);
    }

    [Fact]
    public void ParseText_WithEmptyString_ReturnsEmptyList()
    {
        // Arrange
        var xml = "";

        // Act
        var result = XmlTokenizer.ParseText(xml);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ParseText_WithOnlyWhitespace_ReturnsWhitespaceTokens()
    {
        // Arrange
        var xml = "   \t\n  ";

        // Act
        var result = XmlTokenizer.ParseText(xml);

        // Assert
        Assert.Equal(1, result.Count);
        Assert.Equal(XmlSyntaxKind.Whitespace, result[0].Kind);
    }

    [Fact]
    public void ParseText_WithNestedElements_ReturnsCorrectTokens()
    {
        // Arrange
        var xml = "<outer><inner>text</inner></outer>";
        var expected = new[]
        {
            new XmlToken(new TextSpan(0, 1), XmlSyntaxKind.OpeningAngleBracket),
            new XmlToken(new TextSpan(1, 5), XmlSyntaxKind.ElementName),
            new XmlToken(new TextSpan(6, 1), XmlSyntaxKind.ClosingAngleBracket),
            new XmlToken(new TextSpan(7, 1), XmlSyntaxKind.OpeningAngleBracket),
            new XmlToken(new TextSpan(8, 5), XmlSyntaxKind.ElementName),
            new XmlToken(new TextSpan(13, 1), XmlSyntaxKind.ClosingAngleBracket),
            new XmlToken(new TextSpan(14, 4), XmlSyntaxKind.Text),
            new XmlToken(new TextSpan(18, 1), XmlSyntaxKind.OpeningAngleBracket),
            new XmlToken(new TextSpan(20, 5), XmlSyntaxKind.EndElement),
            new XmlToken(new TextSpan(25, 1), XmlSyntaxKind.ClosingAngleBracket),
            new XmlToken(new TextSpan(26, 1), XmlSyntaxKind.OpeningAngleBracket),
            new XmlToken(new TextSpan(28, 5), XmlSyntaxKind.EndElement),
            new XmlToken(new TextSpan(33, 1), XmlSyntaxKind.ClosingAngleBracket)
        };

        // Act
        var result = XmlTokenizer.ParseText(xml);

        // Assert
        Assert.Equal(expected.Length, result.Count);
        for (var i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i].Span, result[i].Span);
            Assert.Equal(expected[i].Kind, result[i].Kind);
        }
    }
}
