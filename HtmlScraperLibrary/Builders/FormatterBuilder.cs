using HtmlScraperLibrary.Entities;
using HtmlScraperLibrary.Entities.Formatters;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Builders
{
    public class FormatterBuilder
    {
        public static ATextFormatter? BuildFromXml(XElement element)
        {
            return element.Name.LocalName switch
            {
                PrefixFormatter.KEY => new PrefixFormatter(element),
                SuffixFormatter.KEY => new SuffixFormatter(element),
                ReplaceFormatter.KEY => new ReplaceFormatter(element),
                _ => null
            };
        }
    }
}
