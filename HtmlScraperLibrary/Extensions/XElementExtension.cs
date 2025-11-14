using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Extensions
{
    internal static class XElementExtension
    {
        public static List<T> Childs<T>(this XElement element, string nodeName, Func<XElement, T> selector)
        {
            return element.Elements()
                    .Where(e => e.Name == nodeName)
                    .Select(selector)
                    .ToList();
        }
        public static Dictionary<string, string> Dictionnary(this XElement element, string nodeName, Func<XElement, string> keySelector, Func<XElement, string> valueSelector)
        {
            return element.Elements()
                    .Where(e => e.Name == nodeName)
                    .ToDictionary(keySelector, valueSelector);
        }
        public static string Text(this XElement element)
        {
            return string.Concat(element.Nodes().OfType<XText>().Select(t => t.Value));
        }
        public static string StringAttribute(this XElement element, string attributeName)
        {
            return element.Attribute(attributeName)?.Value ?? string.Empty;
        }
        public static bool BooleanAttribute(this XElement element, string attributeName)
        {
            return (element.Attribute(attributeName)?.Value.ToLower() ?? string.Empty) == "true";
        }
    }
}
