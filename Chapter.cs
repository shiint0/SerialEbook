using System.Web;
using HtmlAgilityPack;

namespace SerialEbook
{
    public class Chapter<T> where T : ISerialInfo
    {
        protected T serial;
        protected HtmlDocument htmlDoc = new HtmlDocument();
        public Chapter(T serial, string htmlContent)
        {
            this.serial = serial;
            this.htmlDoc.LoadHtml(htmlContent);            
        }

        protected virtual string FormatBody(string unformatted)
        {
            var decoded = Decode(unformatted);
            var withFixedBrs = FixBrElements(decoded);
            return withFixedBrs;
        }

        protected virtual string FormatTitle(string unformatted)
        {
            var decoded = Decode(unformatted);
            return PerformColectomy(decoded);

        }

        protected string Decode(string html)
        {
            return HttpUtility.HtmlDecode(html);
        }

        protected string PerformColectomy(string htmlWithColons)
        {
            return htmlWithColons.Replace(":", " -");
        }

        protected string FixBrElements(string htmlWithUnterminatedBrs)
        {
            return htmlWithUnterminatedBrs
                .Replace("<br>", "<br/>")
                .Replace("<br >", "<br/>")
                .Replace("< br>", "<br/>")
                .Replace("< br >", "<br/>");
        }

        public string Title => FormatTitle(serial.FindTitle(htmlDoc));

        public string NextChapterUrl => serial.FindNextChapterlink(htmlDoc);

        public string Body => FormatBody(serial.FindBody(htmlDoc));
    }
}