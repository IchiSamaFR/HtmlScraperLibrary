using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlScraperLibrary.Extensions;

namespace HtmlScraperLibrary.Components
{
    /// <summary>
    /// Config file contains all scrapers
    /// </summary>
    [Obsolete]
    public class ComponentConfig
    {
        private readonly XDocument _xDocument;
        private readonly XElement _root;
        private Dictionary<string, string> _properties = null;
        private List<string> _proxies = null;
        private List<ComponentScraper> _scrapers = null;

        public Dictionary<string, string> Properties
        {
            get
            {
                _properties ??= _root.Dictionnary("property", e => e.Attribute("name")?.Value ?? string.Empty, e => e.Value);
                return _properties;
            }
        }

        public List<string> Proxies
        {
            get
            {
                _proxies ??= _root.Childs("proxy", proxy => proxy.Attribute("url")?.Value.ApplyProperties(this) ?? string.Empty);
                return _proxies;
            }
        }

        public List<ComponentScraper> Scrapers
        {
            get
            {
                _scrapers ??= _root.Childs(ComponentScraper.KEY, scraper => new ComponentScraper(scraper, this));
                return _scrapers;
            }
        }

        internal ComponentConfig(XDocument xDocument)
        {
            _xDocument = xDocument;
            _root = _xDocument.Root ?? new XElement("root");
        }
        internal ComponentConfig(Dictionary<string, string> properties)
        {
            _xDocument = new XDocument();
            _root = new XElement("root");
            _properties = properties;
        }

        public static ComponentConfig LoadFromXML(string xmlName)
        {
            return new ComponentConfig(XDocument.Load(xmlName));
        }
        public static ComponentConfig LoadFromXMLContent(string xmlName)
        {
            return new ComponentConfig(XDocument.Parse(xmlName));
        }

        internal string ApplyProperties(string str)
        {
            if (str == null)
            {
                return null;
            }
            foreach (var prop in Properties)
            {
                str = str.Replace("{" + prop.Key + "}", prop.Value);
            }
            return str;
        }

        internal void UpdateProperty(string key, string value)
        {
            if (value == null)
            {
                return;
            }
            _properties ??= new Dictionary<string, string>();
            if (_properties.ContainsKey(key))
            {
                _properties[key] = value;
            }
            else
            {
                _properties.Add(key, value);
            }
        }
        internal void RemoveProperty(string key)
        {
            if (_properties == null || !_properties.ContainsKey(key))
            {
                return;
            }
            _properties.Remove(key);
        }
        internal WebProxy GetProxy()
        {
            string proxy = Proxies.Skip(new Random().Next(0, Proxies.Count)).FirstOrDefault();
            string tmp = "http://" + proxy;

            return string.IsNullOrEmpty(proxy) ? null : new WebProxy(tmp);
        }
    }
}
