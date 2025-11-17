using HtmlScraperLibrary.Extensions;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities.Formatters
{
    public class ReplaceFormatter : ATextFormatter
    {
        public const string KEY = "replace";

        private string _oldValue;
        private string _newValue;
        private string _isRegex;

        public string OldValue
        {
            get => _context?.ApplyProperty(_oldValue) ?? _oldValue;
        }
        public string NewValue
        {
            get => _context?.ApplyProperty(_newValue) ?? _newValue;
        }
        public bool IsRegex
        {
            get => (_context?.ApplyProperty(_isRegex) ?? _isRegex).ToBool();
        }

        public ReplaceFormatter(XElement element)
        {
            _oldValue = element.StringAttribute("oldValue");
            _newValue = element.StringAttribute("newValue");
            _isRegex = element.StringAttribute("isRegex");
        }

        public override string Format(string input)
        {
            if (IsRegex)
                return Regex.Replace(input, OldValue, NewValue);
            else
                return input.Replace(OldValue, NewValue);
        }
    }
}
