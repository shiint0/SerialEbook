using CardboardBox.Epub;
using HtmlAgilityPack;
using SerialEbook;

namespace SerialEbook
{
    public class Pale : Serial
    {
        public Pale(HttpClient httpClient) : base(httpClient)
        {
        }

        public override string Title => "Pale";

        public override string Subtitle => "";

        public override string Author => "Wildbow";

        public override string StartUrl => "https://palewebserial.wordpress.com/2020/05/05/blood-run-cold-0-0/";

        public Func<HtmlDocument, string> FindTitle => htmlDoc => TitleElement(htmlDoc).InnerText;

        protected override Func<HtmlDocument, IEnumerable<HtmlNode>> BodyElements => htmlDoc => htmlDoc.DocumentNode
                                                                        .SelectNodes($"//div[@class='{EntryContentClass}']/p");


        protected override Func<HtmlDocument, string> NextChapterUrl => htmlDoc => htmlDoc.DocumentNode
                                                                                .SelectSingleNode("//span[@class='nav-next']/a")
                                                                                ?.GetAttributeValue<string>("href", "") ?? "";
    }
}