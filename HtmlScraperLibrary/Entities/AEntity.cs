using HtmlAgilityPack;
using HtmlScraperLibrary.Builders;
using HtmlScraperLibrary.Components;
using HtmlScraperLibrary.Extensions;
using HtmlScraperLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public abstract class AEntity
    {
        protected ContextEntity? _context = null!;

        public string Type
        {
            get
            {
                string typeName = GetType().Name;
                if (typeName.EndsWith("Entity"))
                {
                    return typeName.Substring(0, typeName.Length - 6);
                }
                return typeName;
            }
        }
        public string OutputKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string OutputKeyProperty
        {
            get => _context?.ApplyProperty(OutputKey) ?? OutputKey;
        }
        public string NameProperty
        {
            get => _context?.ApplyProperty(Name) ?? Name;
        }

        public AEntity() { }

        public AEntity(XElement element)
        {
            OutputKey = element.StringAttribute(nameof(OutputKey));
            Name = element.StringAttribute(nameof(Name));
        }

        public virtual void LoadContext(ContextEntity context)
        {
            _context = context;
        }
        public virtual void SetProperties()
        {

        }

        public abstract Task Extract(JsonNode jObject, HtmlNode node);
    }
}