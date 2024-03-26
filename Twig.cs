using CardboardBox.Epub;
using HtmlAgilityPack;
using SerialEbook;

namespace SerialEbook
{
    public class Twig : Serial
    {
        public Twig(HttpClient httpClient) : base(httpClient)
        {
        }

        public override string Title => "Twig";

        public override string Subtitle => "";

        public override string Author => "Wildbow";

        public override string StartUrl => "https://twigserial.wordpress.com/2014/12/24/taking-root-1-1/";

        protected override Func<HtmlDocument, string> NextChapterUrl => htmlDoc =>
        {
            var url = htmlDoc.DocumentNode
                        .SelectSingleNode("//a[text()='Next' or descendant::strong[text()='Next']]")
                        ?.GetAttributeValue<string>("href", "");

            return url?.StartsWith("//") == true ? $"https:{url}" : url; // wtf wildbow
        };
        
    }
}