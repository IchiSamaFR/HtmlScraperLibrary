using HtmlAgilityPack;
using HtmlScraperLibrary.Builders;
using HtmlScraperLibrary.Entities.Formatters;
using HtmlScraperLibrary.Extensions;
using System.Formats.Tar;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public class TextEntity : AEntity
    {
        public string Empty { get; set; } = string.Empty;

        public string EmptyProperty
        {
            get => _context?.ApplyProperty(Empty) ?? Empty;
        }

        public List<ATextFormatter> Formatters { get; } = new List<ATextFormatter>();

        public TextEntity() : base() { }
        public TextEntity(XElement element) : base(element)
        {
            Formatters.AddRange(element.Elements().Select(FormatterBuilder.BuildFromXml).Where(n => n != null).ToList()!);
            Empty = element.StringAttribute(nameof(Empty));
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
            var text = node.InnerText;

            foreach (var format in Formatters)
            {
                text = format.Format(text);
            }
            text = string.IsNullOrEmpty(text) ? EmptyProperty : text;

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
