using CardboardBox.Epub;

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
}