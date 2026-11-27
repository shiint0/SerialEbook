using System.Xml;
using System.Diagnostics;
using System.Net.Http;
using System.Collections;
using System.Linq;
using CardboardBox.Epub;
using System.Reflection;
using HtmlAgilityPack;

namespace SerialEbook
{
    class Program
    {
        private static Dictionary<string, Type> serials = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsAssignableTo(typeof(ISerialInfo)) && !t.IsInterface)
                .ToDictionary(t => t.Name, t => t);

        public static void Main(string[] args)
        {
            var success = ProcessInput(args);
            if (!success)
                UsageMessage();
        }

        private static bool ProcessInput(string[] args)
        {
            var nameSuppliedInArgs = args.Length > 0;
            if (!nameSuppliedInArgs)
                return false;

            var suppliedSerialName = args[0];
            var serialImplemented = serials.ContainsKey(suppliedSerialName);
            if (!serialImplemented)
                return false;

            var serialType = serials[suppliedSerialName];

            var serial = serialType?
                .GetConstructor([typeof(HttpClient)])?
                .Invoke([new HttpClient()]) as ISerialInfo;
            
            if (serial == null)
                return false;

            var stopIndexSupplied = args.Length > 1;            
            if (stopIndexSupplied)
            {
                var suppliedStopIndexValue = args[1];
                if (string.IsNullOrWhiteSpace(suppliedStopIndexValue) || !int.TryParse(suppliedStopIndexValue, out var stopIndex))
                    return false;
                ConvertSerial(serial, stopIndex).Wait();
            }
            else
                ConvertSerial(serial).Wait();

            return true;
        }

        private static void UsageMessage()
        {
            Console.WriteLine("Usage: serialEbook <serial name> [stop at chapter].");
            Console.WriteLine("Available serials:");
            serials
                .Select(s => $"   {s}")
                .ToList()
                .ForEach(Console.WriteLine);

        }

        public static async Task ConvertSerial<T>(T serial, int stopAtChapter = 0) where T : ISerialInfo
        {
            var fileName = $"{serial.Title}.epub";

            await using (var epub = EpubBuilder.Create(serial.Title, fileName))
            {
                var builder = await epub.Start();

                builder.Author(serial.Author);

                if (!string.IsNullOrWhiteSpace(serial.StyleSheet))
                    await builder.AddStylesheet("default", serial.StyleSheet);

                Task.WaitAll(serial.ProcessInto(builder).TakeWhile((tast, index) => stopAtChapter == 0 || index + 1 < stopAtChapter).ToEnumerable().ToArray());
            }
        }
    }
}