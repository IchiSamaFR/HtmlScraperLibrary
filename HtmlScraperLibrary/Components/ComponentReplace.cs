using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Components
{
    /// <summary>
    /// Under a ComponentSelector, replace the value to another
    /// </summary>
    [Obsolete]
    public class ComponentReplace
    {
        public const string KEY = "replace";

        private ComponentConfig _config;
        private string _by;

        public Regex Pattern { get; }
        public string By
        {
            get
            {
                return _config.ApplyProperties(_by);
            }
            set
            {
                _by = value;
            }
        }

        public ComponentReplace(XElement e, ComponentConfig config)
        {
            _config = config;
            Pattern = new Regex(e.Attribute("pattern")?.Value ?? "(.*)");
            By = e.Attribute("by")?.Value ?? string.Empty;
        }


        public string Replace(string str)
        {
            return Pattern.Replace(str, By);
        }
    }
}