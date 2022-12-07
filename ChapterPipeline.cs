using System.Net.Http.Headers;
namespace SerialEbook
{
    public class ChapterPipeline<TSerial> : IAsyncEnumerable<Chapter<TSerial>> where TSerial : ISerialInfo
    {
        private readonly TSerial serial;
        protected HttpClient httpClient = new HttpClient();
        public ChapterPipeline(TSerial serial)
        {            
            this.serial = serial;

            // todo: make headers configurable (per serial?)
            httpClient.DefaultRequestHeaders.Add("User-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36");
        }

        public async IAsyncEnumerator<Chapter<TSerial>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var nextChapterUrl = serial.StartUrl;            
            while(nextChapterUrl != "")
            {
                var chapter = new Chapter<TSerial>(serial, await httpClient.GetStringAsync(nextChapterUrl,cancellationToken));
                Console.WriteLine($"Processing chapter {chapter.Title} at {nextChapterUrl}");
                nextChapterUrl = chapter.NextChapterUrl;
                yield return chapter;
            }
            yield break;        
        }       
    }
}