using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Components
{
    /// <summary>
    /// Permit to add a value before
    /// </summary>
    public class ComponentBeforeValue
    {
        public const string KEY = "before";

        protected ComponentConfig _config;
        private string _value;

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

        public ComponentBeforeValue(XElement e, ComponentConfig config)
        {
            _config = config;
            Value = config.ApplyProperties(e.Attribute("value")?.Value);
        }

    }
}
