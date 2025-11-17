using HtmlScraperLibrary.Extensions;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities.Formatters
{
    public class SuffixFormatter : ATextFormatter
    {
        public const string KEY = "suffix";

        private string _value;
        public string Value
        {
            get => _context?.ApplyProperty(_value) ?? _value;
        }
        public SuffixFormatter(XElement element)
        {
            _value = element.StringAttribute("value");
        }
        public override string Format(string input)
        {
            return $"{input}{Value}";
        }
    }
}
