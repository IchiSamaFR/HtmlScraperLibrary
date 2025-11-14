using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace HtmlScraperLibrary.Extensions
{
    internal static class JsonExtension
    {
        public static void ForEach(this JsonObject obj, Action<KeyValuePair<string, JsonNode>> action)
        {
            foreach (var item in obj)
            {
                action?.Invoke(item);
            }
        }

        public static void Merge(this JsonObject target, JsonNode? source)
        {
            if (source is JsonObject sourceObj)
            {
                foreach (var prop in sourceObj)
                {
                    target[prop.Key] = prop.Value;
                }
            }
            else if (source is JsonArray sourceArray)
            {
                // Si on veut fusionner un array dans un objet, on peut choisir une clé spéciale ou ignorer
                // Ici, on ajoute chaque élément sous une clé indexée
                for (int i = 0; i < sourceArray.Count; i++)
                {
                    target[i.ToString()] = sourceArray[i];
                }
            }
            // Sinon, rien à faire pour les autres types
        }
    }
}
