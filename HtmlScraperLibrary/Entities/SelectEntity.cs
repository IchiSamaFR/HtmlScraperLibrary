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

        public SelectEntity(XElement e) : base(e)
        {
            _isArray = e.StringAttribute("isArray");
            _query = e.StringAttribute("query");
        }

        public override async Task Extract(JsonObject jObject, HtmlNode node)
        {
            List<HtmlNode> nodesQuery = new();
            if (IsArray)
            {
                nodesQuery.AddRange(node.QuerySelectorAll(Query));
            }
            else
            {
                nodesQuery.Add(node.QuerySelector(Query));
            }


            // If no children, nothing to extract
            if (Children == null)
            {
                return;
            }

            // Prépare le conteneur de résultat selon IsArray
            JsonNode resultNode = IsArray ? new JsonArray() : new JsonObject();
            foreach (var query in nodesQuery.Where(n => n != null))
            {
                foreach (var child in Children)
                {
                    // On crée un objet temporaire pour chaque enfant
                    var childObj = new JsonObject();
                    await child.Extract(childObj, query);


                    if (IsArray)
                    {
                        // Ajoute une copie profonde pour éviter l'erreur de parent
                        ((JsonArray)resultNode).Add(childObj.DeepClone());
                        continue;
                    }

                    foreach (var prop in childObj)
                    {
                        ((JsonObject)resultNode)[prop.Key] = prop.Value?.DeepClone();
                    }
                }
            }

            // Place le résultat dans le parent si OutputKey est défini
            if (!string.IsNullOrEmpty(OutputKey))
            {
                jObject[OutputKey] = resultNode.DeepClone();
            }
            else if (IsArray)
            {
                jObject.Merge(resultNode);
            }
            else
            {
                foreach (var prop in (JsonObject)resultNode)
                {
                    jObject[prop.Key] = prop.Value?.DeepClone();
                }
            }
        }
    }
}