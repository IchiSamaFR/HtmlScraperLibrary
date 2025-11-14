using HtmlScraperLibrary.Entities;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Builders
{
    public class EntityBuilder
    {
        public static AEntity? BuildFromXml(XElement element)
        {
            return element.Name.LocalName switch
            {
                SelectEntity.KEY => new SelectEntity(element),
                LoopEntity.KEY => new LoopEntity(element),
                WebEntity.KEY => new WebEntity(element),
                TextEntity.KEY => new TextEntity(element),
                _ => null
            };
        }
    }
}
