using HtmlScraperLibrary.Extensions;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities
{
    public class LoopEntity : AParentEntity
    {
        public const string KEY = "loop";

        private string _from;
        private string _to;
        private string _step;

        public int From
        {
            get
            {
                return _context?.ApplyProperty(_from).ToInt() ?? _from.ToInt();
            }
        }
        public int To
        {
            get
            {
                return _context?.ApplyProperty(_to).ToInt() ?? _to.ToInt();
            }
        }
        public int Step
        {
            get
            {
                return _context?.ApplyProperty(_step).ToInt() ?? _step.ToInt();
            }
        }

        public LoopEntity(XElement e) : base(e)
        {
            _from = e.StringAttribute("from");
            _to = e.StringAttribute("to");
            _step = e.StringAttribute("step");
        }
    }
}
