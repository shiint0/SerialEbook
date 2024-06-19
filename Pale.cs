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
                                                                        .SelectNodes("//div[@class='entry-content']/p");


        protected override Func<HtmlDocument, string> NextChapterUrl => htmlDoc => htmlDoc.DocumentNode
                                                                                .SelectSingleNode("//span[@class='nav-next']/a")
                                                                                ?.GetAttributeValue<string>("href", "") ?? "";

        // protected override async Task BuildChapter(IChapterBuilder cb, HtmlDocument doc, int chapterIndex)
        // {
        //     var chapterTitlePagename = $"0-{TitleElement(doc).InnerText}";            
        //     await cb.AddPage(chapterTitlePagename, TitleElement(doc).OuterHtml);

        //     Task.WaitAll(BodyElements(doc)
        //         .Select(async (node, index) => {
        //             var imageSource = node.Element("img")
        //                 ?.GetAttributeValue<string>("src", "")
        //                 ?.Split('?')
        //                 ?.First() ?? "";

        //             if (imageSource != "")
        //             {
        //                 var extension = imageSource.Split('.').Last();
        //                 await cb.AddImage($"{chapterIndex}-img-{index}.{extension}", await httpClient.GetByteArrayAsync(imageSource));
        //             }
        //             else
        //                 await cb.AddPage($"{chapterIndex}-text-{index}", node.InnerHtml);
        //         })
        //         .ToArray()
        //     );
        // }
    }
}