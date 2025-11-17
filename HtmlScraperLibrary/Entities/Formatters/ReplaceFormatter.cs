using HtmlScraperLibrary.Extensions;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities.Formatters
{
    public class ReplaceFormatter : ATextFormatter
    {
        public const string KEY = "Replace";

        public string OldValue{get;set;}
        public string NewValue{get;set;}
        public string IsRegex{get;set;}

        public string OldValueProperty
        {
            get => _context?.ApplyProperty(OldValue) ?? OldValue;
        }
        public string NewValueProperty
        {
            get => _context?.ApplyProperty(NewValue) ?? NewValue;
        }
        public bool IsRegexProperty
        {
            get => (_context?.ApplyProperty(IsRegex) ?? IsRegex).ToBool();
        }

        public ReplaceFormatter(XElement element)
        {
            OldValue = element.StringAttribute(nameof(OldValue));
            NewValue = element.StringAttribute(nameof(NewValue));
            IsRegex = element.StringAttribute(nameof(IsRegex));
        }

        public override string Format(string input)
        {
            if (IsRegexProperty)
                return Regex.Replace(input, OldValueProperty, NewValueProperty);
            else
                return input.Replace(OldValueProperty, NewValueProperty);
        }
    }
}
