using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace HtmlScraperLibrary.Entities
{
    public class RootEntity : AParentEntity
    {
        public ContextEntity Context
        {
            get => _context!;
        }

        public RootEntity() : base() { }
        public RootEntity(XElement element) : base(element)
        {
            LoadContext(new ContextEntity());
        }

        public async Task<JsonObject> Extract(HtmlDocument html)
        {
            var jObject = new JsonObject();
            await Extract(jObject, html.DocumentNode);
            return jObject;
        }
    }
}