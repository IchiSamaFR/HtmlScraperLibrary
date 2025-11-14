using HtmlAgilityPack;
using HtmlScraperLibrary.Builders;
using HtmlScraperLibrary.Extensions;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public class AArrayEntity : AEntity
    {
        private string _isArray;
        public bool IsArray
        {
            get => (_context?.ApplyProperty(_isArray) ?? _isArray).ToBool();
        }

        public List<AEntity>? Children { get; set; } = null;

        public AArrayEntity(XElement e) : base(e)
        {
            _isArray = e.StringAttribute("array");
            Children = e.Elements().Select(EntityBuilder.BuildFromXml).ToList();
        }

        public override async Task Extract(JsonObject jObject, HtmlNode node)
        {
            // Prépare le conteneur de résultat selon IsArray
            JsonNode resultNode = IsArray ? new JsonArray() : new JsonObject();

            // Pour chaque enfant, on extrait et on ajoute au conteneur
            if (Children != null)
            {
                foreach (var child in Children)
                {
                    // On crée un objet temporaire pour chaque enfant
                    var childObj = new JsonObject();
                    await child.Extract(childObj, node);

                    if (IsArray)
                    {
                        // Ajoute une copie profonde pour éviter l'erreur de parent
                        ((JsonArray)resultNode).Add(childObj.DeepClone());
                    }
                    else
                    {
                        foreach (var prop in childObj)
                        {
                            ((JsonObject)resultNode)[prop.Key] = prop.Value?.DeepClone();
                        }
                    }
                }
            }

            // Place le résultat dans le parent si OutputKey est défini
            if (!string.IsNullOrEmpty(OutputKey))
            {
                jObject[OutputKey] = resultNode.DeepClone();
            }
            else
            {
                // Sinon, fusionne directement dans le parent
                if (IsArray)
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
}
