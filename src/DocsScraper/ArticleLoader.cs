using System.Collections.Generic;
using HtmlAgilityPack;

namespace DocsScraper
{
    public abstract class ArticleLoader
    {
        public abstract IEnumerable<ArticleLink> GetLinks(HtmlDocument doc);

        public abstract string GetBodyHtml(HtmlDocument doc);

        public abstract string GetBodyText(HtmlDocument doc);

        public class ArticleLink
        {
            public string Title { get; set; }
            public string Link { get; set; }
        }
    }
}