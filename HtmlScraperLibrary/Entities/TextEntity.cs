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
        public const string KEY = "text";

        public List<ATextFormatter> Formatters { get; } = new List<ATextFormatter>();
        public TextEntity(XElement element) : base(element)
        {
            Formatters.AddRange(element.Elements().Select(FormatterBuilder.BuildFromXml).Where(n => n != null).ToList()!);
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
                if (!string.IsNullOrEmpty(OutputKey))
                {
                    var obj = new JsonObject { [OutputKey] = text };
                    arr.Add(obj);
                }
                else
                {
                    arr.Add(text);
                }
            }
            else if (jObject is JsonObject obj)
            {
                if (!string.IsNullOrEmpty(OutputKey))
                {
                    obj[OutputKey] = text;
                }
                else
                {
                    obj[KEY] = text;
                }
            }

            return Task.CompletedTask;
        }
    }
}
