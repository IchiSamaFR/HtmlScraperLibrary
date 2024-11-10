using System;
using System.Collections.Generic;
using System.Xml.Linq;
using HtmlScraperLibrary.Extensions;

namespace HtmlScraperLibrary.Components
{
    /// <summary>
    /// Component allows you to precisely select the node on which you want to work as a funnel
    /// </summary>
    public class ComponentSelect
    {
        public const string KEY = "select";

        private ComponentConfig _config;
        private string _query;
        private string _to;


        public string Query
        {
            get
            {
                return _config.ApplyProperties(_query);
            }
            set
            {
                _query = value;
            }
        }
        public string To
        {
            get
            {
                return _config.ApplyProperties(_to);
            }
            set
            {
                _to = value;
            }
        }
        public List<ComponentSelect> Childs { get; }
        public List<AComponentValue> Text { get; }
        public bool IsSingle { get; }
        public List<ComponentWeb> Web { get; }

        public ComponentSelect(XElement e, ComponentConfig config)
        {
            _config = config;

            Query = e.Attribute("query")?.Value ?? string.Empty;
            To = e.Attribute("to")?.Value ?? "select-" + Guid.NewGuid().ToString();
            Childs = e.Childs(KEY, e => new ComponentSelect(e, config));
            Text = new List<AComponentValue>();
            Text.AddRange(e.Childs(ComponentText.KEY, e => new ComponentText(e, config)));
            Text.AddRange(e.Childs(ComponentAttribute.KEY, e => new ComponentAttribute(e, config)));
            Text.AddRange(e.Childs(ComponentValue.KEY, e => new ComponentValue(e, config)));
            Web = e.Childs("web", e => new ComponentWeb(e, config));

            IsSingle = e.BooleanAttribute("single");
        }
    }
}