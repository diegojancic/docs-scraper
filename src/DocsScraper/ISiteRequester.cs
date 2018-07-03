using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DocsScraper
{
    /// <summary>
    /// Loads information from the endpoint (ie, HTTP). This class must be thread safe
    /// </summary>
    public interface ISiteRequester
    {
        string GetHtml(string url);
    }
}
