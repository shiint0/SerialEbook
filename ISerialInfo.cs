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
        Func<HtmlDocument, string> FindTitle { get; }
        Func<HtmlDocument, string> FindBody { get; }
        Func<HtmlDocument, string> FindNextChapterlink { get; }
    }
}