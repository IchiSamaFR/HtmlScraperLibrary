using HtmlAgilityPack;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public class TextEntity : AEntity
    {
        public const string KEY = "text";

        public TextEntity(XElement element) : base(element)
        {
        }

        public override Task Extract(JsonObject jObject, HtmlNode node)
        {
            if (!string.IsNullOrEmpty(OutputKey))
            {
                jObject[OutputKey] = node.InnerHtml;
            }
            return Task.CompletedTask;
        }
    }
}
