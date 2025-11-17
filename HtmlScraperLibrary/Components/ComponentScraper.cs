using System.Collections.Generic;
using System.Xml.Linq;
using HtmlScraperLibrary.Extensions;

namespace HtmlScraperLibrary.Components
{
    /// <summary>
    /// Scraper contains all loops for a scrap
    /// </summary>
    [Obsolete]
    public class ComponentScraper
    {
        public const string KEY = "scraper";

        public ComponentConfig Config { get; set; }
        public string Name { get; private set; }
        public ComponentSelect Select { get; private set; }
        public List<ComponentLoop> List { get; private set; }
        public List<ComponentBlacklist> Blacklist { get; private set; }

        public ComponentScraper(XElement e, ComponentConfig context)
        {
            Config = context;
            Name = e.Attribute("name")?.Value ?? string.Empty;
            List = e.Childs(ComponentLoop.KEY, list => new ComponentLoop(list, context));
            Blacklist = e.Childs(ComponentBlacklist.KEY, blacklist => new ComponentBlacklist(blacklist, context));
        }
    }
}
