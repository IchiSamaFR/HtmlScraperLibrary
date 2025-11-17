using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Text.Json.Nodes;
using HtmlScraperLibrary.Extensions;
using HtmlScraperLibrary.Components;

namespace HtmlScraperLibrary
{
    [Obsolete]
    internal static class HTMLToolLocal
    {
        public static JsonObject StartAnalyse(HtmlDocument doc, ComponentSelect select, JsonObject o = null)
        {
            o ??= new JsonObject();
            AnalyseNode(doc.DocumentNode, select, o);
            return o;
        }
        private static JsonObject AnalyseNode(HtmlNode node, ComponentSelect select, JsonObject o = null)
        {
            o ??= new JsonObject();
            foreach (var text in select.Text)
            {
                AnalyseText(node, text, o);
            }
            foreach (var subSelect in select.Childs)
            {
                AnalyseSelect(node, subSelect, o);
            }
            foreach (var web in select.Web)
            {
                o.Add("webNode", "No data.");
            }
            return o;
        }
        private static void AnalyseText(HtmlNode node, AComponentValue text, JsonObject o)
        {
            if (node == null)
            {
                return;
            }
            o.Add(text.To, text.Extract(node));
        }
        private static void AnalyseSelect(HtmlNode node, ComponentSelect select, JsonObject o)
        {
            if (node == null)
            {
                return;
            }
            if (select.IsSingle)
            {
                AnalyseNode(node.QuerySelector(select.Query), select, o);
            }
            else
            {
                var suba = new JsonArray();
                foreach (var subnode in node.QuerySelectorAll(select.Query))
                {
                    suba.Add(AnalyseNode(subnode, select));
                }
                o.Add(select.To, suba);
            }
        }


        public static async Task<JsonObject> StartAnalyseAsync(HtmlDocument doc, ComponentSelect select, JsonObject o = null)
        {
            o ??= new JsonObject();
            await AnalyseNodeAsync(doc.DocumentNode, select, o);
            await TaskExtension.WaitUntilEnd();
            return o;
        }
        public static async Task<JsonObject> AnalyseNodeAsync(HtmlNode node, ComponentSelect select, JsonObject o = null)
        {
            o ??= new JsonObject();
            foreach (var text in select.Text)
            {
                o.Add(text.To, text.Extract(node));
            }
            foreach (var subSelect in select.Childs)
            {
                AnalyseSelectAsync(node, subSelect, o);
            }
            foreach (var web in select.Web)
            {
                o.Add("webNode", "No data.");
            }
            return o;
        }
        public static async void AnalyseSelectAsync(HtmlNode node, ComponentSelect select, JsonObject o)
        {
            if (node == null)
            {
                return;
            }
            if (select.IsSingle)
            {
                await AnalyseNodeAsync(node.QuerySelector(select.Query), select, o);
            }
            else
            {
                var suba = new JsonArray();
                foreach (var subnode in node.QuerySelectorAll(select.Query))
                {
                    suba.Add(AnalyseNodeAsync(subnode, select));
                }
                o.Add(select.To, suba);
            }
        }
    }
}

