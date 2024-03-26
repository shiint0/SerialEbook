using System.Xml;
using System.Diagnostics;
using System.Net.Http;
using System.Collections;
using System.Linq;
using CardboardBox.Epub;

namespace SerialEbook
{
    class Program
    {
        public static void Main(string[] args)
        {
            var serial = new Twig(new HttpClient());
            Task.WaitAll(ConvertSerial(serial));
        }

        public static async Task ConvertSerial<T>(T serial) where T : ISerialInfo
        {            
            var fileName = $"{serial.Title}.epub";

            await using (var epub = EpubBuilder.Create(serial.Title, fileName))
            {
                var builder = await epub.Start();

                builder.Author(serial.Author);
                
                if((serial.StyleSheet ?? "") != "")
                    await builder.AddStylesheet("default", serial.StyleSheet);

                Task.WaitAll(serial.ProcessInto(builder).ToEnumerable().ToArray());
            }            
        }
    }
}