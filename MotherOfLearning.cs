using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace SerialEbook
{
    public class MotherOfLearning : Serial
    {
        public MotherOfLearning(HttpClient httpClient) : base(httpClient)
        {
        }

        public override string Title => "Mother of Learning";

        public override string Subtitle => "chapter-inner chapter-content";

        public override string Author => "nobody103";

        protected override string EntryContentClass => "chapter-inner chapter-content";
        public override string StartUrl => "https://www.royalroad.com/fiction/21220/mother-of-learning/chapter/301778/1-good-morning-brother";
            
        protected override Func<HtmlDocument, HtmlNode> TitleElement => htmlDoc => htmlDoc.DocumentNode
                                                                                    .SelectSingleNode($"//div[@class='{EntryContentClass}']/p/strong[2]");
        protected override Func<HtmlDocument, IEnumerable<HtmlNode>> BodyElements => htmlDoc =>
        {
            var piracyMessageStyleElement = htmlDoc.DocumentNode.SelectSingleNode("//style[contains(text(), 'display: none;')]").InnerText;
            var piracyMessageClass = Regex.Match(piracyMessageStyleElement, @"\.([a-zA-Z0-9]+)\s*\{").Groups.Values.Skip(1).FirstOrDefault()?.Value ?? "";
            return htmlDoc.DocumentNode
                                                                                    .SelectNodes($"//div[@class='{EntryContentClass}']/p[preceding::p]")
                                                                                    .Where(n => !n.HasClass(piracyMessageClass));
        };

        protected override Func<HtmlDocument, string> NextChapterUrl => htmlDoc =>
        {
            var relativeUrl = htmlDoc.DocumentNode
                                .SelectSingleNode($"//div[@class='row nav-buttons']/div/a[contains(text(),'Next')]")
                                ?.GetAttributeValue<string>("href", "") ?? "";
            
            return relativeUrl == "" ? "" : $"https://www.royalroad.com{relativeUrl}";
        };
    }
}