using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlScraperLibrary.Interfaces
{
    public interface IContextProperties
    {
        string? GetProperty(string key);
        void SetProperty(string key, string value);
    }
}
