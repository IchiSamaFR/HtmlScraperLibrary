using HtmlScraperLibrary.Extensions;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities.Formatters
{
    public class PrefixFormatter : ATextFormatter
    {
        public string Value { get; set; } = string.Empty;

        public string ValueProperty
        {
            get => _context?.ApplyProperty(Value) ?? Value;
        }

        public PrefixFormatter() { }
        public PrefixFormatter(XElement element)
        {
            Value = element.StringAttribute(nameof(Value));
        }

        public override string Format(string input)
        {
            return $"{ValueProperty}{input}";
        }
    }
}
