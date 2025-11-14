using HtmlAgilityPack;
using HtmlScraperLibrary.Builders;
using HtmlScraperLibrary.Extensions;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public class AParentEntity : AEntity
    {
        public List<AEntity>? Children { get; set; } = null;

        public AParentEntity(XElement e) : base(e)
        {
            Children = e.Elements().Select(EntityBuilder.BuildFromXml).Where(n => n != null).ToList()!;
        }

        public override async Task Extract(JsonObject jObject, HtmlNode node)
        {
            // If no children, nothing to extract
            if (Children == null)
            {
                return;
            }

            // Prépare le conteneur de résultat selon IsArray
            JsonNode resultNode = new JsonObject();
            foreach (var child in Children)
            {
                // On crée un objet temporaire pour chaque enfant
                var childObj = new JsonObject();
                await child.Extract(childObj, node);

                foreach (var prop in childObj)
                {
                    ((JsonObject)resultNode)[prop.Key] = prop.Value?.DeepClone();
                }
            }

            // Place le résultat dans le parent si OutputKey est défini
            if (!string.IsNullOrEmpty(OutputKey))
            {
                jObject[OutputKey] = resultNode.DeepClone();
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
