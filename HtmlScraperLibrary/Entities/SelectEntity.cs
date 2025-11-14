using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using HtmlScraperLibrary.Extensions;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public class SelectEntity : AArrayEntity
    {
        public const string KEY = "select";

        private string _query;

        public string Query
        {
            get => _context?.ApplyProperty(_query) ?? _query;
        }

        public SelectEntity(XElement e) : base(e)
        {
            _query = e.StringAttribute("query");
        }

        public override async Task Extract(JsonObject jObject, HtmlNode node)
        {
            List<HtmlNode> value = new();
            if (IsArray)
            {
                value.AddRange(node.QuerySelectorAll(Query));
            }
            else
            {
                value.Add(node.QuerySelector(Query));
            }

            await base.Extract(jObject, value.First());
        }
    }
}