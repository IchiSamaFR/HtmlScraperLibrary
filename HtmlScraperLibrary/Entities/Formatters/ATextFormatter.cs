using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HtmlScraperLibrary.Entities.Formatters
{
    public abstract class ATextFormatter
    {
        protected ContextEntity? _context = null!;

        public void LoadContext(ContextEntity context)
        {
            _context = context;
        }
        public abstract string Format(string input);
    }
}
