using HtmlAgilityPack;

namespace SerialEbook
{
    public class MyDiscipleDiedYetAgain : ISerialInfo
    {
        public string Title => "My Disciple Died Yet Again";

        public string Subtitle => string.Empty;

        public string Author => "Scrya_Translations"; // todo: add translator field, find real author

        public string StartUrl => "https://www.scribblehub.com/read/574082-my-disciple-died-yet-again/chapter/574107";

        public string StyleSheet => "#chp_contents span { font-family: 'Open Sans',sans-serif!important; font-size: 15px; } .chapter-title ｛　text-align: center;　font-size: 24px;　font-weight: 700;　font-family: 'Open Sans',serif;　line-height: 30px;　padding-bottom: 10px;　｝｝";

        private Func<HtmlDocument, HtmlNode> TitleElement => htmlDoc => htmlDoc.DocumentNode.SelectSingleNode("//div[@class=\"chapter-title\"]");
        public Func<HtmlDocument, string> FindTitle => htmlDoc => TitleElement(htmlDoc).InnerText.Replace($"{Title} &#8211; ", "");

        public Func<HtmlDocument, string> FindBody => htmlDoc => htmlDoc.DocumentNode.SelectNodes("//div[@id=\"chp_raw\"]/p").Select(n => n.OuterHtml).Aggregate(string.Concat);

        public Func<HtmlDocument, string> FindNextChapterlink => htmlDoc => htmlDoc.DocumentNode.SelectSingleNode("//a[@class=\"btn-wi btn-next\"]")?.Attributes.FirstOrDefault(a => a.Name == "href")?.Value ?? "";
    }
}