using HtmlAgilityPack;
using HtmlScraperLibrary.Builders;
using HtmlScraperLibrary.Entities.Formatters;
using System.Formats.Tar;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public class TextEntity : AEntity
    {
        public List<ATextFormatter> Formatters { get; } = new List<ATextFormatter>();
        public TextEntity(XElement element) : base(element)
        {
            Formatters.AddRange(element.Elements().Select(FormatterBuilder.BuildFromXml).Where(n => n != null).ToList()!);
        }

        public override void LoadContext(ContextEntity context)
        {
            base.LoadContext(context);
            foreach (var formatter in Formatters)
            {
                formatter.LoadContext(context);
            }
        }

        public override Task Extract(JsonNode jObject, HtmlNode node)
        {
            var text = node.InnerHtml;

            foreach (var format in Formatters)
            {
                text = format.Format(text);
            }

            if (jObject is JsonArray arr)
            {
                if (!string.IsNullOrEmpty(OutputKeyProperty))
                {
                    var obj = new JsonObject { [OutputKeyProperty] = text };
                    arr.Add(obj);
                }
                else
                {
                    arr.Add(text);
                }
            }
            else if (jObject is JsonObject obj)
            {
                if (!string.IsNullOrEmpty(OutputKeyProperty))
                {
                    obj[OutputKeyProperty] = text;
                }
            }

            return Task.CompletedTask;
        }
    }
}
