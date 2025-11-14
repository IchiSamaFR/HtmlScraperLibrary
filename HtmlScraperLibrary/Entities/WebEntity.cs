using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public class WebEntity : AArrayEntity
    {
        public const string KEY = "web";

        public WebEntity(XElement e) : base(e)
        {
        }
    }
}
