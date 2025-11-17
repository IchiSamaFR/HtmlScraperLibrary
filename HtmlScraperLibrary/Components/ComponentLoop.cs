using System.Xml.Linq;
using System.Linq;
using System;

namespace HtmlScraperLibrary.Components
{

    /// <summary>
    /// Pages
    /// </summary>
    [Obsolete]
    public class ComponentLoop
    {
        public const string KEY = "loop";

        private ComponentConfig _config;
        private string _loopName;
        private string _loopStart;
        private string _loopEnd;

        public ComponentWeb Web { get; private set; } = null!;
        public string LoopName
        {
            get
            {
                return _config.ApplyProperties(_loopName);
            }
            set
            {
                _loopName = value;
            }
        }
        public string LoopStart
        {
            get
            {
                return _config.ApplyProperties(_loopStart);
            }
            set
            {
                _loopStart = value;
            }
        }
        public string LoopEnd
        {
            get
            {
                return _config.ApplyProperties(_loopEnd);
            }
            set
            {
                _loopEnd = value;
            }
        }

        private ComponentLoop()
        {

        }
        internal ComponentLoop(XElement e, ComponentConfig config)
        {
            _config = config;
            var loop = e.Attribute("loop")?.Value.Split(':');
            LoopName = loop?[0];
            LoopStart = loop?[1];
            LoopEnd = loop?[2];
            Web = BuildWeb(e, config);
        }

        private static ComponentWeb BuildWeb(XElement e, ComponentConfig context)
        {
            var web = e.Elements().FirstOrDefault(child => child.Name == ComponentWeb.KEY);
            return web == null ? null : new ComponentWeb(web, context);
        }
    }
}