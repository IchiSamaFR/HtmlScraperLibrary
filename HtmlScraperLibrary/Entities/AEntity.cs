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

        private string _outputKey = string.Empty;

        public string OutputKey
        {
            get => _context?.ApplyProperty(_outputKey) ?? _outputKey;
        }

        public AEntity(XElement element)
        {
            _outputKey = element.StringAttribute("outputKey");
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