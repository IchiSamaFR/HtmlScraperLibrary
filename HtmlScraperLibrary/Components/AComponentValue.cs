using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlScraperLibrary.Components
{
    /// <summary>
    /// ComponentSelector is an abstract class to be the one which get the value
    /// </summary>
    public abstract class AComponentValue
    {
        protected ComponentConfig _config;
        private string _to;

        public Regex Regex { get; protected set; }
        public string Property { get; protected set; }
        public bool IsTrim { get; protected set; }
        public bool IsHtmlDecode { get; protected set; }
        public string To
        {
            get
            {
                return _config.ApplyProperties(_to);
            }
            set
            {
                _to = value;
            }
        }
        public string Empty { get; protected set; }
        public List<ComponentReplace> Replace { get; protected set; }
        public List<ComponentBeforeValue> BeforeValues { get; protected set; }

        /// <summary>
        /// Return the value from a node
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public abstract string Extract(HtmlNode e);
    }
}
