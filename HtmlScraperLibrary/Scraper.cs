using System.Diagnostics;
using System.Text.Json.Nodes;
using HtmlAgilityPack;
using HtmlScraperLibrary.Components;
using HtmlScraperLibrary.Extensions;

namespace HtmlScraperLibrary
{
    public class Scraper
    {
        internal static string CHROME_PATH = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";

        public Scraper(ComponentConfig context)
        {
            _config = context;
        }

        private ComponentConfig _config { get; set; }

        public static Scraper Build(ComponentConfig context)
        {
            return new Scraper(context);
        }

        public JsonArray Action(ComponentScraper contextScraper)
        {
            var res = new JsonArray();
            var localContext = contextScraper.Config;
            foreach (var list in contextScraper.List)
            {
                if (list.LoopName != null)
                {
                    for (var loopIndex = list.LoopStart.ToInt(); loopIndex <= list.LoopEnd.ToInt(); loopIndex++)
                    {
                        var timer = new Stopwatch();
                        timer.Start();

                        localContext.UpdateProperty(list.LoopName, loopIndex.ToString());
                        res.Add(GetNodeResult(list.Web));
                        localContext.RemoveProperty(list.LoopName);

                        timer.Stop();
                        TimeSpan timeTaken = timer.Elapsed;
                        Console.WriteLine($"Page {loopIndex} : {timeTaken.ToString(@"m\:ss\.fff")}m");
                    }
                }
                else
                {
                    res.Add(GetNodeResult(list.Web));
                }
            }

            CleanBlacklist(contextScraper, res);

            return res;
        }
        public async Task<JsonArray> ActionAsync(ComponentScraper contextScraper)
        {
            var res = new JsonArray();
            var localContext = contextScraper.Config;
            foreach (var list in contextScraper.List)
            {
                if (list.LoopName != null)
                {
                    for (var loopIndex = list.LoopStart.ToInt(); loopIndex <= list.LoopEnd.ToInt(); loopIndex++)
                    {
                        var timer = new Stopwatch();
                        timer.Start();

                        localContext.UpdateProperty(list.LoopName, loopIndex.ToString());
                        res.Add(await GetNodeResultAsync(list.Web));
                        localContext.RemoveProperty(list.LoopName);

                        timer.Stop();
                        TimeSpan timeTaken = timer.Elapsed;
                        Console.WriteLine($"Page {loopIndex} : {timeTaken.ToString(@"m\:ss\.fff")}m");
                    }
                }
                else
                {
                    res.Add(await GetNodeResultAsync(list.Web));
                }
            }

            CleanBlacklist(contextScraper, res);

            return res;
        }


        public JsonArray ActionLocal(ComponentScraper contextScraper, HtmlDocument htmlDocument)
        {
            var res = new JsonArray();
            var localContext = contextScraper.Config;
            foreach (var list in contextScraper.List)
            {
                if (list.LoopName != null)
                {
                    var timer = new Stopwatch();
                    timer.Start();

                    localContext.UpdateProperty(list.LoopName, "0");
                    res.Add(GetNodeLocalResult(list.Web, htmlDocument));
                    localContext.RemoveProperty(list.LoopName);

                    timer.Stop();
                    TimeSpan timeTaken = timer.Elapsed;
                    Console.WriteLine($"Local page : {timeTaken.ToString(@"m\:ss\.fff")}m");
                }
                else
                {
                    res.Add(GetNodeLocalResult(list.Web, htmlDocument));
                }
            }

            CleanBlacklist(contextScraper, res);

            return res;
        }
        public async Task<JsonObject> GetNodeResultAsync(ComponentWeb web, JsonObject o = null)
        {
            o ??= new JsonObject();
            var doc = web.Send();
            return await HTMLTool.StartAnalyseAsync(doc, web.Select, o);
        }
        public JsonObject GetNodeResult(ComponentWeb web, JsonObject o = null)
        {
            o ??= new JsonObject();
            var doc = web.Send();
            return HTMLTool.StartAnalyse(doc, web.Select, o);
        }
        public JsonObject GetNodeLocalResult(ComponentWeb web, HtmlDocument htmlDocument, JsonObject o = null)
        {
            o ??= new JsonObject();
            return HTMLToolLocal.StartAnalyse(htmlDocument, web.Select, o);
        }
        private void CleanBlacklist(ComponentScraper contextScraper, JsonArray res)
        {
            if (!contextScraper.Blacklist.Any())
            {
                return;
            }

            List<JsonNode> lsts = new List<JsonNode>();
            foreach (JsonArray lst in res.Select(obj => obj["lst"]))
            {
                List<JsonObject> toRemove = new List<JsonObject>();
                foreach (JsonObject item in lst.Where(obj => contextScraper.Blacklist.Any(bl => obj[bl.Variable].ToString() == bl.Value)))
                {
                    toRemove.Add(item);
                }
                foreach (var removed in toRemove)
                {
                    lst.Remove(removed);
                    lsts.Add(JsonNode.Parse(removed.ToString()));
                }
            }
        }

        public void ConfigureChromePath(string path)
        {
            CHROME_PATH = path;
        }
    }
}
