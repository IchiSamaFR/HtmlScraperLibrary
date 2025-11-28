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

        public string Type
        {
            get
            {
                string typeName = GetType().Name;
                if (typeName.EndsWith("Formatter"))
                {
                    return typeName.Substring(0, typeName.Length - 9);
                }
                return typeName;
            }
        }
        public void LoadContext(ContextEntity context)
        {
            _context = context;
        }
        public abstract string Format(string input);
    }
}
