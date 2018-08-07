using System.IO;

namespace Crawler.Test.PageProcessors.HtmlPages
{
    public class ReadTextFromHtml
    {
        public static string FromHtmlFile(string fileName)
        {
            string filePath = $"{Directory.GetCurrentDirectory()}\\HtmlPages\\{fileName}";
            string page = File.ReadAllText(filePath);
            return page;
        }
    }
}