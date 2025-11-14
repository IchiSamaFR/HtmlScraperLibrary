using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using System;
using HtmlScraperLibrary.Extensions;

namespace HtmlScraperLibrary.Components
{
    /// <summary>
    /// Herited from ComponentSelector
    /// - Get the value from a plain text
    /// </summary>
    public class ComponentValue : AComponentValue
    {
        public const string KEY = "value";

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

        public ComponentValue(XElement e, ComponentConfig config)
        {
            _config = config;

            Value = e.Attribute(KEY)?.Value ?? string.Empty;
            Regex = new Regex(e.Attribute("regex")?.Value ?? "(.*)");
            IsTrim = e.BooleanAttribute("trim");
            IsHtmlDecode = e.BooleanAttribute("htmldecode");
            To = e.Attribute("to")?.Value ?? "text-" + Guid.NewGuid().ToString();
            Property = e.Attribute("property")?.Value ?? string.Empty;
            Empty = e.Attribute("empty")?.Value ?? string.Empty;
            Replace = e.Childs(ComponentReplace.KEY, replace => new ComponentReplace(replace, config));
        }
        public override string Extract(HtmlNode e)
        {
            return Regex
                .Matches(Value)
                .FirstOrDefault(m => true)?
                .Groups[1]?.Value
                .HTMLDecode(IsHtmlDecode)
                .Trim(IsTrim)
                .Replace(Replace) ?? Empty;
        }
    }
}