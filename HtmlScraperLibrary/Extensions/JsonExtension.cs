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
    }
}
