using HtmlAgilityPack;

namespace SerialEbook
{
    public class SuperSupportive : Serial
    {
        public SuperSupportive(HttpClient httpClient) : base(httpClient)
        {
        }

        public override string Title => "Super Supportive";

        public override string Subtitle => "";

        public override string Author => "Sleyca";

        public override string StartUrl => "https://www.royalroad.com/fiction/63759/super-supportive/chapter/1096481/one-the-boy-in-the-bubble";

        protected override string EntryContentClass => "chapter-inner chapter-content";

        protected override Func<HtmlDocument, IEnumerable<HtmlNode>> BodyElements => htmlDoc => htmlDoc.DocumentNode
                                                                        .SelectNodes($"//div[@class='{EntryContentClass}']/*[normalize-space()]")
                                                                        .Where(n => !n.HasClass(RoyalRoadPiracyClass(htmlDoc)));
        protected override Func<HtmlDocument, HtmlNode> TitleElement => htmlDoc => htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='font-white break-word']");

        protected override Func<HtmlDocument, string> NextChapterUrl => doc =>
        {
            var relativeUrl = doc.DocumentNode
                                .SelectSingleNode("//a[descendant::i[@class='far fa-chevron-double-right ml-3']]")?
                                .GetAttributeValue<string>("href", "") ?? "";

            return relativeUrl == "" ? "" : $"https://www.royalroad.com{relativeUrl}";
        };

    }
}