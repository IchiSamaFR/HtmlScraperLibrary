using HtmlAgilityPack;
using PuppeteerSharp;
using System.Net;
using System.Xml.Linq;
using HtmlScraperLibrary.Extensions;

namespace HtmlScraperLibrary.Components
{
    [Obsolete]
    public class ComponentWeb
    {
        public const string KEY = "web";

        private ComponentConfig _config;
        private string _url = string.Empty;
        private List<Cookie> _cookies = new List<Cookie>();
        private Dictionary<string, string> _headers = new Dictionary<string, string>();
        private Dictionary<string, string> _localProperties = new Dictionary<string, string>();

        public string Url
        {
            get
            {
                return _config.ApplyProperties(ApplyLocalProperties(_url));
            }
            set
            {
                _url = value;
            }
        }
        public bool Chrome { get; private set; }
        public HttpMethod Method { get; private set; }
        public Dictionary<string, string> Headers
        {
            get
            {
                var tmp = new Dictionary<string, string>();
                foreach (var item in _headers)
                {
                    tmp.Add(_config.ApplyProperties(ApplyLocalProperties(item.Key)), _config.ApplyProperties(ApplyLocalProperties(item.Value)));
                }
                return tmp;
            }
            set
            {
                _headers = value;
            }
        }
        public List<Cookie> Cookies
        {
            get
            {
                return _cookies.Select(c => new Cookie(_config.ApplyProperties(ApplyLocalProperties(c.Name)),
                                                       _config.ApplyProperties(ApplyLocalProperties(c.Value)),
                                                       _config.ApplyProperties(ApplyLocalProperties(c.Path)),
                                                       _config.ApplyProperties(ApplyLocalProperties(c.Domain)))).ToList();
            }
            set
            {
                _cookies = value;
            }
        }
        public ComponentSelect Select { get; private set; } = null!;

        internal ComponentWeb(ComponentConfig config, string url, bool chrome, Dictionary<string, string> header, List<Cookie> cookies, ComponentSelect select)
        {
            _config = config;
            Url = url;
            Chrome = chrome;
            Method = HttpMethod.Get;
            Headers = header;
            Cookies = cookies;
            Select = select;
        }
        internal ComponentWeb(XElement e, ComponentConfig config)
        {
            _config = config;
            Url = e.Attribute("url")?.Value ?? string.Empty;
            Chrome = e.BooleanAttribute("chrome");
            Method = HttpMethod.Get;
            Headers = e.Dictionnary("header", e => e.Attribute("name")?.Value ?? string.Empty, e => e.Attribute("value")?.Value ?? string.Empty);
            Cookies = e.Childs("cookie", e => new Cookie(e.Attribute("name")?.Value,
                                                         e.Attribute("value")?.Value,
                                                         e.Attribute("path")?.Value,
                                                         e.Attribute("domain")?.Value));
            Select = new ComponentSelect(e, config);
        }
        public ComponentWeb Copy()
        {
            return new ComponentWeb(_config, _url, Chrome, _headers, _cookies, Select);
        }

        #region Properties
        private string ApplyLocalProperties(string str)
        {
            if (str == null)
            {
                return null;
            }
            foreach (var prop in _localProperties)
            {
                str = str.Replace("{" + prop.Key + "}", prop.Value);
            }
            return str;
        }
        internal void UpdateProperty(string key, string value)
        {
            if (value == null)
            {
                return;
            }
            _localProperties ??= new Dictionary<string, string>();
            if (_localProperties.ContainsKey(key))
            {
                _localProperties[key] = value;
            }
            else
            {
                _localProperties.Add(key, value);
            }
        }
        internal void RemoveProperty(string key)
        {
            if (_localProperties == null || !_localProperties.ContainsKey(key))
            {
                return;
            }
            _localProperties.Remove(key);
        }
        #endregion

        #region Send
        public HtmlDocument Send(WebProxy proxy = null)
        {
            var doc = new HtmlDocument();
            if (Chrome)
            {
                doc.LoadHtml(ChromeSend(proxy).Result);
            }
            else
            {
                doc.LoadHtml(HttpClientSend(proxy).Result);
            }
            return doc;
        }
        private async Task<string> HttpClientSend(WebProxy proxy)
        {
            var container = new CookieContainer();
            Cookies.ForEach(c => container.Add(c));

            var _client = new HttpClient(new HttpClientHandler
            {
                Proxy = _config.GetProxy(),
                UseCookies = true,
                CookieContainer = container
            });

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(Url),
                Method = Method,
            };
            foreach (var kv in Headers)
            {
                httpRequestMessage.Headers.Add(kv.Key, kv.Value);
            }

            Console.WriteLine(">>" + Method.ToString() + " " + Url);
            var response = await _client.SendAsync(httpRequestMessage);
            Console.WriteLine("<<" + response.StatusCode);
            return response.Content.Text();
        }
        private async Task<string> ChromeSend(WebProxy proxy)
        {
            BrowserFetcher browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            var options = new LaunchOptions()
            {
                ExecutablePath = Scraper.CHROME_PATH,
                Headless = false,
                Args = new string[] {
                }
            };
            var browser = await Puppeteer.LaunchAsync(options);
            var page = await browser.NewPageAsync();
            await page.SetRequestInterceptionAsync(true);
            page.Request += (obj, arg) =>
            {
                var lst = new List<string>() { "image", "stylesheet", "font" };
                if (lst.Contains(arg.Request.ResourceType.ToString().ToLower()))
                {
                    arg.Request.AbortAsync();
                }
                else
                {
                    arg.Request.ContinueAsync();
                }
            };

            await page.SetCookieAsync(Cookies.CookieToParm().ToArray());
            try
            {
                await page.GoToAsync(Url);
            }
            catch
            {
                return string.Empty;
            }
            var content = await page.GetContentAsync();
            await browser.CloseAsync();
            return content;
        }
        #endregion
    }
}
