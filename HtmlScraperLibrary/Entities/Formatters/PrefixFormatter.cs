using HtmlScraperLibrary.Extensions;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities.Formatters
{
    public class PrefixFormatter : ATextFormatter
    {
        public const string KEY = "prefix";

        private string _value;
        public string Value
        {
            get => _context?.ApplyProperty(_value) ?? _value;
        }
        public PrefixFormatter(XElement element)
        {
            _value = element.StringAttribute("value");
        }
        public override string Format(string input)
        {
            return $"{Value}{input}";
        }
    }
}
