using System;
using System.Collections.Generic;

namespace ZohoKBScraper
{
    public class Scraper
    {
        public string Url { get; set; }

        /// <summary>
        /// Creates a new scraper for the Zoho KB.
        /// </summary>
        /// <param name="url">URL of the Zoho KB that contains a list of articles</param>
        public Scraper(string url)
        {
            Url = url;
        }

        public IList<Article> GetArticles()
        {
            return new List<Article>();
        }
    }
}
