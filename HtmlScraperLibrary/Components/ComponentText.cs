﻿using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using System;
using HtmlScraperLibrary.Extensions;

namespace HtmlScraperLibrary.Components
{
    /// <summary>
    /// Herited from ComponentSelector
    /// - Get the value from a plain text
    /// </summary>
    public class ComponentText : AComponentValue
    {
        public const string KEY = "text";

        public bool IsContain { get; protected set; }

        public ComponentText(XElement e, ComponentConfig config)
        {
            _config = config;
            Regex = new Regex(e.Attribute("regex")?.Value ?? "(.*)");
            RegexGroup = e.Attribute("regexgroup")?.Value ?? "1";
            IsTrim = e.BooleanAttribute("trim");
            IsHtmlDecode = e.BooleanAttribute("htmldecode");
            IsContain = e.BooleanAttribute("iscontain");
            To = e.Attribute("to")?.Value ?? "text-" + Guid.NewGuid().ToString();
            Property = e.Attribute("property")?.Value ?? string.Empty;
            Empty = e.Attribute("empty")?.Value ?? string.Empty;
            Replace = e.Childs(ComponentReplace.KEY, replace => new ComponentReplace(replace, config));
            BeforeValues = e.Childs(ComponentBeforeValue.KEY, before => new ComponentBeforeValue(before, config));
        }
        public override string Extract(HtmlNode e)
        {
            if (e == null)
            {
                return string.Empty;
            }
            var tmp = Regex
                .Matches(string.Join("", e.ChildNodes.Select(node => node.InnerText)).Trim())
                .FirstOrDefault(m => true);
            var val = Regex
                .Matches(string.Join("", e.ChildNodes.Select(node => node.InnerText)).Trim())
                .FirstOrDefault(m => true)?
                .Groups[RegexGroup]?.Value
                .HTMLDecode(IsHtmlDecode)
                .Trim(IsTrim)
                .Replace(Replace) ?? Empty;
            return IsContain ? ReturnAsLiquid(val) : val;
        }
        private string ReturnAsLiquid(string val)
        {
            var isLiter = !val.ToLower().Contains("cl");

            val = val.Replace("c", "").Replace("l", "");
            if (isLiter)
            {
                var tmp = val.Replace(',', '.').Split('.');
                val = tmp[0] + (tmp.Count() > 1 ? tmp[1].Right(2, '0') : "00");
            }

            return val;
        }
    }
}