using HtmlScraperLibrary.Entities;
using HtmlScraperLibrary.Entities.Formatters;
using System.Xml.Linq;
using System.Reflection;

namespace HtmlScraperLibrary.Builders
{
    public class FormatterBuilder
    {
        private const string FormatterSuffix = "Formatter";
        private static readonly Dictionary<string, Type> _formatterTypes;

        static FormatterBuilder()
        {
            _formatterTypes = Assembly.GetAssembly(typeof(ATextFormatter))!
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(ATextFormatter).IsAssignableFrom(t))
                .ToDictionary(
                    t => t.Name.EndsWith(FormatterSuffix) ? t.Name[..^FormatterSuffix.Count()] : t.Name,
                    t => t,
                    StringComparer.OrdinalIgnoreCase
                );
        }

        public static ATextFormatter? BuildFromXml(XElement element)
        {
            if (_formatterTypes.TryGetValue(element.Name.LocalName, out var type))
            {
                var ctor = type.GetConstructor(new[] { typeof(XElement) });
                if (ctor != null)
                {
                    return (ATextFormatter)ctor.Invoke(new object[] { element });
                }
            }
            return null;
        }
    }
}