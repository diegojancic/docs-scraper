using System.IO;
using System.Net;

namespace DocsScraper
{
    public class HttpSiteRequester : ISiteRequester
    {
        public string GetHtml(string url)
        {
            var req = WebRequest.Create(url);
            var response = req.GetResponse();
            string html;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                html = reader.ReadToEnd();
            }

            return html;
        }
    }
}