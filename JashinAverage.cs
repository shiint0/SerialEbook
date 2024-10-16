using HtmlAgilityPack;

namespace SerialEbook
{
    public class JashinAverage : Serial
    {
        public JashinAverage(HttpClient httpClient) : base(httpClient)
        {
        }

        public override string Title => "Jashin Average";

        public override string Subtitle => "";

        public override string Author => "Kitaseno Yunaki";

        public override string StartUrl => "https://oniichanyamete.moe/2015/04/15/jashin-average-1/";

        protected override Func<HtmlDocument, HtmlNode> TitleElement => htmlDoc => htmlDoc.DocumentNode
                                                                                    .SelectSingleNode($"//div[@class='{EntryContentClass}']/*[starts-with(name(), 'h') and (starts-with(text(), 'Chapter ') or starts-with(text(), 'Side '))]");

        protected override Func<HtmlDocument, string> NextChapterUrl => htmlDoc => htmlDoc.DocumentNode
                                                                                    .SelectSingleNode($"//div[@class='{EntryContentClass}']/p/a[text()='Next Chapter']")
                                                                                    ?.GetAttributeValue<string>("href","")
                                                                                    ?.Replace("wordpress.com", "moe") ?? "";
    }
}