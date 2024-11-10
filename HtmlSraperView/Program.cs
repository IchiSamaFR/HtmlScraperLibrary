using HtmlAgilityPack;
using HtmlScraperLibrary;
using HtmlScraperLibrary.Components;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static void Main(string[] args)
    {
        RunLocal();

        Console.ReadLine();
    }
    private static void RunLocal()
    {
        var document = new HtmlDocument();
        document.LoadHtml(File.ReadAllText("Configs/google.html"));

        var context = ComponentConfig.LoadFromXML("Configs/googleConfig.xml");
        var scrapper = Scraper.Build(context);
        foreach (var item in context.Scrapers)
        {
            Console.WriteLine(scrapper.ActionLocal(item, document));
        }
    }
    private static void RunWeb()
    {
        var context = ComponentConfig.LoadFromXML("Configs/googleConfig.xml");
        var scrapper = Scraper.Build(context);
        foreach (var item in context.Scrapers)
        {
            Console.WriteLine(scrapper.Action(item));
        }
    }
}