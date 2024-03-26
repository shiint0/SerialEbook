using HtmlAgilityPack;

namespace SerialEbook
{
    public class PracticalGuideToEvil : Serial
    {
        public PracticalGuideToEvil(HttpClient httpClient) : base(httpClient)
        {
        }

        public override string Title => "A Practical Guide to Evil";
        public override string Subtitle => "Do Wrong Rightt";
        public override string Author => "ErraticErrata";
        public override string StartUrl => "https://practicalguidetoevil.wordpress.com/2015/03/25/prologue/";
        public override string StyleSheet => ".wf-active body, .wf-active button, .wf-active input, .wf-active select, .wf-active textarea{font-family:\"Noto Sans\",sans-serif}.wf-active blockquote{font-family:\"Noto Sans\",sans-serif}.wf-active button, .wf-active input[type=\"button\"], .wf-active input[type=\"reset\"], .wf-active input[type=\"submit\"]{font-family:\"Noto Sans\",sans-serif}.wf-active .widget_search .search-field{font-family:\"Noto Sans\",sans-serif}.wf-active .widget_search .search-submit{font-family:\"Noto Sans\",sans-serif}.wf-active #infinite-handle span{font-family:\"Noto Sans\",sans-serif}.wf-active h1{font-family:\"Noto Sans\",sans-serif;font-weight:400;font-style:normal}.wf-active h2{font-family:\"Noto Sans\",sans-serif;font-weight:400;font-style:normal}.wf-active h3{font-family:\"Noto Sans\",sans-serif;font-weight:400;font-style:normal}.wf-active h4{font-family:\"Noto Sans\",sans-serif;font-style:normal;font-weight:400}.wf-active h5{font-family:\"Noto Sans\",sans-serif;font-style:normal;font-weight:400}.wf-active h6{font-family:\"Noto Sans\",sans-serif;font-weight:400;font-style:normal}.wf-active .widget-title{font-family:\"Noto Sans\",sans-serif;font-weight:400;font-style:normal}.wf-active .entry-title{font-family:\"Noto Sans\",sans-serif;font-weight:400;font-style:normal}.wf-active .page-title{font-weight:400;font-style:normal}.wf-active .format-aside .entry-title, .wf-active .format-quote .entry-title{font-style:normal;font-weight:400}.wf-active .site-title{font-weight:400;font-family:\"Noto Sans\",sans-serif;font-style:normal}.wf-active .site-description{font-family:\"Noto Sans\",sans-serif;font-weight:400;font-style:normal}.wf-active .comments-title{font-weight:400;font-style:normal}";

        protected override Func<HtmlDocument, HtmlNode> TitleElement => htmlDoc => htmlDoc.DocumentNode.SelectSingleNode("//h1[@class=\"entry-title\"]");
        public Func<HtmlDocument, string> FindNextChapterlink => htmlDoc => htmlDoc.DocumentNode.SelectSingleNode("//div[@class=\"nav-next\"]/a")?.Attributes.FirstOrDefault(a => a.Name == "href")?.Value ?? "";
    }
}