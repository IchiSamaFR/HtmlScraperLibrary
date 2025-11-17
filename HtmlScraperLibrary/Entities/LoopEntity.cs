using HtmlAgilityPack;
using HtmlScraperLibrary.Extensions;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public class LoopEntity : AParentEntity
    {
        public string From { get; set; } = "0";
        public string To { get; set; } = "0";
        public string Step { get; set; } = "0";
        public int ActualStep { get; set; }

        public int FromProperty
        {
            get => _context?.ApplyProperty(From).ToInt() ?? From.ToInt();
        }
        public int ToProperty
        {
            get => _context?.ApplyProperty(To).ToInt() ?? To.ToInt();
        }
        public int StepProperty
        {
            get => _context?.ApplyProperty(Step).ToInt() ?? Step.ToInt();
        }
        public int ActualStepProperty
        {
            get => ActualStep;
        }

        public LoopEntity() : base() { }
        public LoopEntity(XElement e) : base(e)
        {
            From = e.StringAttribute(nameof(From));
            To = e.StringAttribute(nameof(To));
            Step = e.StringAttribute(nameof(Step));
            Step = string.IsNullOrEmpty(Step) ? "1" : Step;
        }

        public override void SetProperties()
        {
            if (_context == null)
                return;

            if (string.IsNullOrEmpty(NameProperty))
            {
                return;
            }
            _context?.SetProperty($"{Name}.{nameof(From)}", From.ToString());
            _context?.SetProperty($"{Name}.{nameof(To)}", To.ToString());
            _context?.SetProperty($"{Name}.{nameof(Step)}", Step.ToString());
            _context?.SetProperty($"{Name}.{nameof(ActualStep)}", ActualStep.ToString());
        }

        public override async Task Extract(JsonNode jNode, HtmlNode node)
        {
            if (Children == null || Children.Count == 0)
                return;

            if (!string.IsNullOrEmpty(OutputKeyProperty))
            {
                // Utilise un tableau pour collecter les résultats de la boucle
                for (ActualStep = FromProperty; ActualStep < ToProperty; ActualStep += StepProperty)
                {
                    SetProperties();
                    var arrayResult = new JsonArray();
                    foreach (var child in Children)
                    {
                        await child.Extract(arrayResult, node);
                    }

                    if (jNode is JsonObject obj)
                    {
                        obj[OutputKeyProperty] = arrayResult;
                    }
                    else if (jNode is JsonArray arr)
                    {
                        arr.Add(new JsonObject { [OutputKeyProperty] = arrayResult });
                    }
                }
            }
            else
            {
                // Passe directement jNode aux enfants pour chaque itération
                for (int i = FromProperty; i < ToProperty; i += StepProperty)
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
