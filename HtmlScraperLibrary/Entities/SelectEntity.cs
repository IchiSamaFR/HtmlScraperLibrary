using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using HtmlScraperLibrary.Extensions;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public class SelectEntity : AParentEntity
    {
        public const string KEY = "Select";

        public string IsArray { get; set; }
        public string Query { get; set; }

        public bool IsArrayProperty
        {
            get => (_context?.ApplyProperty(IsArray) ?? IsArray).ToBool();
        }
        public string QueryProperty
        {
            get => _context?.ApplyProperty(Query) ?? Query;
        }

        public SelectEntity(XElement element) : base(element)
        {
            IsArray = element.StringAttribute(nameof(IsArray));
            Query = element.StringAttribute(nameof(Query));
        }

        public override async Task Extract(JsonNode jObject, HtmlNode node)
        {
            if (Children == null || Children.Count == 0)
                return;

            List<HtmlNode> nodesQuery = new();
            if (string.IsNullOrEmpty(QueryProperty))
            {
                nodesQuery.Add(node);
            }
            else if (IsArrayProperty)
            {
                nodesQuery.AddRange(node.QuerySelectorAll(QueryProperty));
            }
            else
            {
                nodesQuery.Add(node.QuerySelector(QueryProperty));
            }

            if (!string.IsNullOrEmpty(OutputKeyProperty))
            {
                // On prépare le conteneur de résultat selon IsArray
                JsonNode resultNode = IsArrayProperty ? new JsonArray() : new JsonObject();

                foreach (var query in nodesQuery.Where(n => n != null))
                {
                    foreach (var child in Children)
                    {
                        await child.Extract(resultNode, query);
                    }
                }

                jObject[OutputKeyProperty] = resultNode.DeepClone();
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