using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace HtmlScraperLibrary.Entities
{
    public class RootEntity : AParentEntity
    {
        public const string KEY = "root";

        public RootEntity(XElement element) : base(element)
        {
        }

        public async Task<JsonObject> Extract(HtmlDocument html)
        {
            var jObject = new JsonObject();
            await Extract(jObject, html.DocumentNode);
            return jObject;
        }
    }
}