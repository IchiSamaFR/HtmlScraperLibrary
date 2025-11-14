namespace HtmlScraperLibrary.Entities
{
    public class ContextEntity
    {
        protected Dictionary<string, string> _properties = new();

        internal string? GetProperty(string key)
        {
            if (_properties.ContainsKey(key))
            {
                return _properties[key];
            }
            return string.Empty;
        }

        internal void SetProperty(string key, string value)
        {
            _properties[key] = value;
        }

        internal string ApplyProperty(string? str)
        {
            if (str == null)
            {
                return string.Empty;
            }
            foreach (var prop in _properties)
            {
                str = str.Replace("{" + prop.Key + "}", prop.Value);
            }
            return str;
        }
    }
}
