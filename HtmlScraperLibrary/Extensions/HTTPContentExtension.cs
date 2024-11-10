using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HtmlScraperLibrary.Extensions
{
    internal static class HTTPContentExtension
    {
        public static string Text(this HttpContent response)
        {
            return new StreamReader(response.ReadAsStream(), Encoding.UTF8).ReadToEnd();
        }
        public static CookieParam CookieToParm(Cookie cookie)
        {
            return new CookieParam()
            {
                Domain = cookie.Domain,
                Path = cookie.Path,
                Value = cookie.Value,
                Name = cookie.Name
            };
        }
        public static IEnumerable<CookieParam> CookieToParm(this List<Cookie> cookie)
        {
            return cookie.Select(c => CookieToParm(c));
        }
    }
}
