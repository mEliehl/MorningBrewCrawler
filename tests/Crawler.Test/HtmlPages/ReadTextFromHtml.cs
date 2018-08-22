using System.IO;

namespace Crawler.Test.PageProcessors.HtmlPages
{
    public class ReadTextFromHtml
    {
        public static string FromHtmlFile(string fileName)
        {
            string @base = $"{Directory.GetCurrentDirectory()}";
            string filePath = Path.Combine(@base,"HtmlPages",fileName);            
            string page = File.ReadAllText(filePath);
            return page;
        }
    }
}