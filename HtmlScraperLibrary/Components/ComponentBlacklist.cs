using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Components
{
    public class ComponentBlacklist
    {
        public const string KEY = "select";

        private ComponentConfig _config;
        private string _variable;
        private string _value;

        public string Variable
        {
            get
            {
                return _config.ApplyProperties(_variable);
            }
            set
            {
                _variable = value;
            }
        }
        public string Value
        {
            get
            {
                return _config.ApplyProperties(_value);
            }
            set
            {
                _value = value;
            }
        }

        public ComponentBlacklist(XElement e, ComponentConfig config)
        {
            _config = config;
            Variable = config.ApplyProperties(e.Attribute("variable")?.Value);
            Value = config.ApplyProperties(e.Attribute("value")?.Value);
        }
    }
}
