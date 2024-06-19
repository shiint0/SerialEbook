using System.Web;
using CardboardBox.Epub;
using HtmlAgilityPack;

namespace SerialEbook
{
    public interface ISerialInfo
    {
        string Title { get; }
        string Subtitle { get; }
        string Author { get; }
        string StartUrl { get; }
        string StyleSheet { get; }            
        IAsyncEnumerable<Task> ProcessInto(IEpubBuilder epubBuilder);
    }

    public abstract class Serial : ISerialInfo
    {        
        public abstract string Title { get; }
        public abstract string Subtitle { get; }
        public abstract string Author { get; }
        public abstract string StartUrl { get; }
        public virtual string StyleSheet => ":root { --font-headings: unset; --font-base: unset; --font-headings-default: -apple-system,BlinkMacSystemFont,\"Segoe UI\",Roboto,Oxygen-Sans,Ubuntu,Cantarell,\"Helvetica Neue\",sans-serif; --font-base-default: -apple-system,BlinkMacSystemFont,\"Segoe UI\",Roboto,Oxygen-Sans,Ubuntu,Cantarell,\"Helvetica Neue\",sans-serif;}";
        protected HttpClient httpClient;

        protected Serial(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        protected virtual string FormatBodyElement(string unformatted)
        {
            var decoded = Decode(unformatted);
            var withFixedBrs = FixBrElements(decoded);
            return withFixedBrs;
        }


        protected virtual string FormatChapterTitle(string unformatted)
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
        protected virtual Func<HtmlDocument, HtmlNode> TitleElement => htmlDoc => htmlDoc.DocumentNode.SelectSingleNode("//h1[@class=\"entry-title\"]");
        protected virtual Func<HtmlDocument, string> FindChapterTitle => htmlDoc => TitleElement(htmlDoc).InnerText;
        protected Func<HtmlDocument, string> ChapterTitle => htmlDoc => FormatChapterTitle(FindChapterTitle(htmlDoc));
        protected virtual Func<HtmlDocument, IEnumerable<HtmlNode>> BodyElements => htmlDoc => htmlDoc.DocumentNode
                                                                        .SelectNodes("//div[@class='entry-content']/p[not(descendant::a)]");

        protected virtual Func<HtmlDocument, string> NextChapterUrl => htmlDoc => htmlDoc.DocumentNode
                                                                                    .SelectSingleNode("//div[@class='entry-content']//a[text()='Next']")
                                                                                    ?.GetAttributeValue<string>("href","") ?? "";

        protected virtual async Task<string> LoadChapter(string url, int retry = 0)
        {
            try
            {
                return await httpClient.GetStringAsync(url);
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                if (e.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    await Task.Delay(1000);
                
                if(retry <= 10)
                    return await LoadChapter(url, retry+1);

                throw;                
            }
        }

        protected virtual Task BuildChapter(IChapterBuilder cb, HtmlDocument doc, int chapterIndex)
        {
            return cb.AddPage($"{chapterIndex}", TitleElement(doc).OuterHtml + "\n" 
                                                    + BodyElements(doc)
                                                    .Select(n => FormatBodyElement(n.OuterHtml)).Aggregate(string.Concat));
        }    

        public async IAsyncEnumerable<Task> ProcessInto(IEpubBuilder epubBuilder) 
        {
            var chapterUrl = StartUrl;
            var chapterIndex = 0;
            while (!string.IsNullOrEmpty(chapterUrl))
            {
                var chapterHtml = await LoadChapter(chapterUrl);
                var chapter = new HtmlDocument();
                chapter.LoadHtml(chapterHtml);
                Console.WriteLine($"Processing chapter {chapterIndex} {ChapterTitle(chapter)} at {chapterUrl}");
                yield return epubBuilder.AddChapter(ChapterTitle(chapter), async cb => await BuildChapter(cb, chapter, chapterIndex));
                chapterUrl = NextChapterUrl(chapter);
                chapterIndex++;
            }
        }
    }
}