using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DocsScraper
{
    public interface ISiteRequester
    {
        string GetHtml(string url);
    }
}
