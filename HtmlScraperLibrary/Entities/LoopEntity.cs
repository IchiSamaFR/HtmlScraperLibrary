using HtmlAgilityPack;
using HtmlScraperLibrary.Extensions;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public class LoopEntity : AParentEntity
    {
        public const string KEY = "loop";

        private string _from;
        private string _to;
        private string _step;

        public int From
        {
            get => _context?.ApplyProperty(_from).ToInt() ?? _from.ToInt();
        }
        public int To
        {
            get => _context?.ApplyProperty(_to).ToInt() ?? _to.ToInt();
        }
        public int Step
        {
            get => _context?.ApplyProperty(_step).ToInt() ?? _step.ToInt();
        }

        public string ContextFromKey;
        public string ContextToKey;
        public string ContextStepKey;

        public LoopEntity(XElement e) : base(e)
        {
            _from = e.StringAttribute("from");
            _to = e.StringAttribute("to");
            _step = e.StringAttribute("step");
            _step = string.IsNullOrEmpty(_step) ? "1" : _step;
            ContextFromKey = e.StringAttribute("contextFromKey");
            ContextToKey = e.StringAttribute("contextToKey");
            ContextStepKey = e.StringAttribute("contextStepKey");
        }

        public override void SetProperties()
        {
            if (_context == null)
                return;

            if (!string.IsNullOrEmpty(ContextFromKey))
            {
                _context?.SetProperty(ContextFromKey, From.ToString());
            }
            if (!string.IsNullOrEmpty(ContextToKey))
            {
                _context?.SetProperty(ContextToKey, To.ToString());
            }
            if (!string.IsNullOrEmpty(ContextStepKey))
            {
                _context?.SetProperty(ContextStepKey, Step.ToString());
            }
        }

        public override async Task Extract(JsonNode jNode, HtmlNode node)
        {
            if (Children == null || Children.Count == 0)
                return;

            if (!string.IsNullOrEmpty(OutputKey))
            {
                // Utilise un tableau pour collecter les résultats de la boucle
                for (int i = From; i < To; i += Step)
                {
                    SetProperties();
                    var arrayResult = new JsonArray();
                    foreach (var child in Children)
                    {
                        await child.Extract(arrayResult, node);
                    }

                    if (jNode is JsonObject obj)
                    {
                        obj[OutputKey] = arrayResult;
                    }
                    else if (jNode is JsonArray arr)
                    {
                        arr.Add(new JsonObject { [OutputKey] = arrayResult });
                    }
                }
            }
            else
            {
                // Passe directement jNode aux enfants pour chaque itération
                for (int i = From; i < To; i += Step)
                {
                    foreach (var child in Children)
                    {
                        await child.Extract(jNode, node);
                    }
                }
            }
        }
    }
}
