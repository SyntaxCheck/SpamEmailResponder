using HtmlAgilityPack;
using System;
using System.IO;

public class HtmlConvert
{
    public HtmlConvert()
    {
    }

    public string Convert(string path)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.Load(path);

        StringWriter sw = new StringWriter();
        ConvertTo(doc.DocumentNode, sw);
        sw.Flush();
        return sw.ToString();
    }

    public string ConvertHtml(string html)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);

        StringWriter sw = new StringWriter();
        ConvertTo(doc.DocumentNode, sw);
        sw.Flush();
        return sw.ToString();
    }

    private void ConvertContentTo(HtmlNode node, TextWriter outText)
    {
        foreach (HtmlNode subnode in node.ChildNodes)
        {
            ConvertTo(subnode, outText);
        }
    }

    public void ConvertTo(HtmlNode node, TextWriter outText)
    {
        string html;
        switch (node.NodeType)
        {
            case HtmlNodeType.Comment:
                // don't output comments
                break;

            case HtmlNodeType.Document:
                ConvertContentTo(node, outText);
                break;

            case HtmlNodeType.Text:
                // script and style must not be output
                string parentName = node.ParentNode.Name;
                if ((parentName == "script") || (parentName == "style"))
                    break;

                // get text
                html = ((HtmlTextNode)node).Text;

                // is it in fact a special closing node output as text?
                if (HtmlNode.IsOverlappedClosingElement(html))
                    break;

                // check the text is meaningful and not a bunch of whitespaces
                if (html.Trim().Length > 0)
                {
                    outText.Write(HtmlEntity.DeEntitize(html));
                }
                break;

            case HtmlNodeType.Element:
                switch (node.Name)
                {
                    case "p":
                        // treat paragraphs as crlf
                        outText.Write(Environment.NewLine);
                        break;
                    case "br":
                        // treat br element as crlf
                        outText.Write(Environment.NewLine);
                        break;
                    case "pre":
                        // treat pre element as crlf
                        outText.Write(Environment.NewLine);
                        break;
                    case "h1":
                        // treat h1 element as crlf
                        outText.Write(Environment.NewLine);
                        break;
                    case "h2":
                        // treat h2 element as crlf
                        outText.Write(Environment.NewLine);
                        break;
                    case "h3":
                        // treat h3 element as crlf
                        outText.Write(Environment.NewLine);
                        break;
                    case "h4":
                        // treat h4 element as crlf
                        outText.Write(Environment.NewLine);
                        break;
                    case "h5":
                        // treat h5 element as crlf
                        outText.Write(Environment.NewLine);
                        break;
                }

                if (node.HasChildNodes)
                {
                    ConvertContentTo(node, outText);
                }
                break;
        }
    }
}
