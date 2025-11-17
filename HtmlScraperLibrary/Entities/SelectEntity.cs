using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using HtmlScraperLibrary.Extensions;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public class SelectEntity : AParentEntity
    {
        private string _isArray;
        public bool IsArray
        {
            get => (_context?.ApplyProperty(_isArray) ?? _isArray).ToBool();
        }

        public const string KEY = "select";

        private string _query;

        public string Query
        {
            get => _context?.ApplyProperty(_query) ?? _query;
        }

        public SelectEntity(XElement element) : base(element)
        {
            _isArray = element.StringAttribute("isArray");
            _query = element.StringAttribute("query");
        }

        public override async Task Extract(JsonNode jObject, HtmlNode node)
        {
            if (Children == null || Children.Count == 0)
                return;

            List<HtmlNode> nodesQuery = new();
            if (string.IsNullOrEmpty(Query))
            {
                nodesQuery.Add(node);
            }
            else if (IsArray)
            {
                nodesQuery.AddRange(node.QuerySelectorAll(Query));
            }
            else
            {
                nodesQuery.Add(node.QuerySelector(Query));
            }

            if (!string.IsNullOrEmpty(OutputKey))
            {
                // On prépare le conteneur de résultat selon IsArray
                JsonNode resultNode = IsArray ? new JsonArray() : new JsonObject();

                foreach (var query in nodesQuery.Where(n => n != null))
                {
                    foreach (var child in Children)
                    {
                        await child.Extract(resultNode, query);
                    }
                }

                jObject[OutputKey] = resultNode.DeepClone();
            }
            else
            {
                // Passe directement jObject aux enfants
                foreach (var query in nodesQuery.Where(n => n != null))
                {
                    foreach (var child in Children)
                    {
                        await child.Extract(jObject, query);
                    }
                }
            }
        }
    }
}