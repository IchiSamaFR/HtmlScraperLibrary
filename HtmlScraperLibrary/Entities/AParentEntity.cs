using HtmlAgilityPack;
using HtmlScraperLibrary.Builders;
using HtmlScraperLibrary.Extensions;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public abstract class AParentEntity : AEntity
    {
        public List<AEntity> Children { get; } = new List<AEntity>();

        public AParentEntity() { }
        public AParentEntity(XElement element) : base(element)
        {
            Children.AddRange(element.Elements().Select(EntityBuilder.BuildFromXml).Where(n => n != null).ToList()!);
        }

        public override void LoadContext(ContextEntity context)
        {
            base.LoadContext(context);
            foreach (var child in Children)
            {
                child.LoadContext(context);
            }
        }

        public override async Task Extract(JsonNode jObject, HtmlNode node)
        {
            if (Children == null)
                return;

            if (!string.IsNullOrEmpty(OutputKeyProperty))
            {
                // Utilise un objet temporaire pour collecter les résultats
                var resultNode = new JsonObject();
                foreach (var child in Children)
                {
                    await child.Extract(resultNode, node);
                }
                jObject[OutputKeyProperty] = resultNode.DeepClone();
            }
            else
            {
                // Passe directement jObject aux enfants
                foreach (var child in Children)
                {
                    await child.Extract(jObject, node);
                }
            }
        }
    }
}
