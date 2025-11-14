using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using HtmlScraperLibrary.Components;

namespace HtmlScraperLibrary.Extensions
{
    internal static class StringExtension
    {
        public static string ApplyProperties(this string str, ComponentConfig context)
        {
            return context?.ApplyProperties(str) ?? str;
        }
        public static string StripSpaces(this string str, bool active)
        {
            return active ? Regex.Replace(str, @"\s", "") : str;
        }
        public static string Trim(this string str, bool active)
        {
            return active ? str.Trim() : str;
        }
        public static string HTMLDecode(this string str, bool active)
        {
            return active ? HttpUtility.HtmlDecode(str) : str;
        }
        public static string Replace(this string str, List<ComponentReplace> contextReplace)
        {
            foreach (var replace in contextReplace)
            {
                str = replace.Replace(str);
            }
            return str;
        }

        public static int ToInt(this string str)
        {
            if (int.TryParse(str, out int result))
            {
                return result;
            }
            return 0;
        }
        public static bool ToBool(this string str)
        {
            if (bool.TryParse(str, out bool result))
            {
                return result;
            }
            return false;
        }
        public static decimal ToDecimal(this string str)
        {
            str = str.Replace(".", ",");
            if (decimal.TryParse(str, out decimal result))
            {
                return result;
            }
            return 0;
        }
    }

}
