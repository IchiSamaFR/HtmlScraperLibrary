using HtmlScraperLibrary.Extensions;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities.Formatters
{
    public class PrefixFormatter : ATextFormatter
    {
        public const string KEY = "Prefix";

        private string Value;
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
