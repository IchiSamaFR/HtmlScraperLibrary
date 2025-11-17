using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public class WebEntity : AParentEntity
    {
        public WebEntity() : base() { }
        public WebEntity(XElement element) : base(element)
        {
        }
    }
}
