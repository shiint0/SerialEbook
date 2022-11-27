namespace SerialEbook
{
    public class ChapterPipeline<TSerial> : IAsyncEnumerable<Chapter<TSerial>> where TSerial : ISerialInfo
    {
        private readonly TSerial serial;
        protected HttpClient httpClient = new HttpClient();    
        public ChapterPipeline(TSerial serial)
        {            
            this.serial = serial;
        }

        public async IAsyncEnumerator<Chapter<TSerial>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var nextChapterUrl = serial.StartUrl;            
            while(nextChapterUrl != "")
            {
                var chapter = new Chapter<TSerial>(serial, await httpClient.GetStringAsync(nextChapterUrl));
                Console.WriteLine($"Processing chapter {chapter.Title} at {nextChapterUrl}");
                nextChapterUrl = chapter.NextChapterUrl;
                yield return chapter;
            }
            yield break;        
        }       
    }
}