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
    /// - Get the value from an attribute
    /// </summary>
    public class ComponentAttribute : AComponentValue
    {
        public const string KEY = "attribute";

        public ComponentAttribute(XElement e, ComponentConfig config)
        {
            _config = config;
            Attribute = e.Attribute(KEY)?.Value ?? string.Empty;
            Regex = new Regex(e.Attribute("regex")?.Value ?? "(.*)");
            RegexGroup = e.Attribute("regexgroup")?.Value ?? "1";
            IsTrim = e.BooleanAttribute("trim");
            IsHtmlDecode = e.BooleanAttribute("htmldecode");
            To = e.Attribute("to")?.Value ?? "text-" + Guid.NewGuid().ToString();
            Property = e.Attribute("property")?.Value ?? string.Empty;
            Empty = e.Attribute("empty")?.Value ?? string.Empty;
            Replace = e.Childs(ComponentReplace.KEY, replace => new ComponentReplace(replace, config));
            BeforeValues = e.Childs(ComponentBeforeValue.KEY, before => new ComponentBeforeValue(before, config));
        }

        public string Attribute { get; }

        public override string Extract(HtmlNode e)
        {
            if (e == null)
            {
                return string.Empty;
            }
            var tmpBefore = string.Join("", BeforeValues.Select(v => v.Value));
            return Regex
                .Matches(string.Join("", tmpBefore, e.GetAttributes(Attribute).FirstOrDefault()?.Value))
                .FirstOrDefault(m => true)?
                .Groups[RegexGroup]?.Value
                .HTMLDecode(IsHtmlDecode)
                .Trim(IsTrim)
                .Replace(Replace) ?? Empty;
        }
    }
}