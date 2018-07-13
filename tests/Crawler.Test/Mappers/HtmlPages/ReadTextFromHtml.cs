using System.IO;

namespace Crawler.Test.Mappers.HtmlPages
{
    public class ReadTextFromHtml
    {
        public static string FromHtmlFile(string fileName)
        {
            string filePath = $"{Directory.GetCurrentDirectory()}\\Mappers\\HtmlPages\\{fileName}";
            string page = File.ReadAllText(filePath);
            return page;
        }
    }
}