using HtmlAgilityPack;
using SerialEbook;

namespace serialEbook
{
    public class Twig : ISerialInfo
    {
        public string Title => "Twig";

        public string Subtitle => "";

        public string Author => "Wildbow";

        public string StartUrl => "https://twigserial.wordpress.com/2014/12/24/taking-root-1-1/";

        public string StyleSheet => ":root { --font-headings: unset; --font-base: unset; --font-headings-default: -apple-system,BlinkMacSystemFont,\"Segoe UI\",Roboto,Oxygen-Sans,Ubuntu,Cantarell,\"Helvetica Neue\",sans-serif; --font-base-default: -apple-system,BlinkMacSystemFont,\"Segoe UI\",Roboto,Oxygen-Sans,Ubuntu,Cantarell,\"Helvetica Neue\",sans-serif;}";
        private Func<HtmlDocument, HtmlNode> TitleElement => htmlDoc => htmlDoc.DocumentNode.SelectSingleNode("//h1[@class=\"entry-title\"]");
        public Func<HtmlDocument, string> FindTitle => htmlDoc => TitleElement(htmlDoc).InnerText;

        public Func<HtmlDocument, string> FindBody => htmlDoc => TitleElement(htmlDoc).OuterHtml + "\n" 
                                                                    + htmlDoc.DocumentNode
                                                                        .SelectNodes("//div[@class='entry-content']/p[not(descendant::a) or normalize-space() != '']")                                                                        
                                                                        .Select(n => n.OuterHtml).Aggregate(string.Concat);

        public Func<HtmlDocument, string> FindNextChapterlink => htmlDoc => htmlDoc.DocumentNode.SelectSingleNode("//div[@class='entry-content']//a[text()='Next']").GetAttributeValue<string>("href","");

    }
}