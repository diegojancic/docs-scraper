using System.Collections.Generic;
using HtmlAgilityPack;

namespace DocsScraper
{
    /// <summary>
    /// Abtract article parser and loader.
    /// </summary>
    public abstract class ArticleLoader
    {
        /// <summary>
        /// Returns a list of articles (including title and url)
        /// </summary>
        /// <param name="doc">Root HtmlDocument</param>
        /// <returns></returns>
        public abstract IEnumerable<ArticleLink> GetLinks(HtmlDocument doc);

        /// <summary>
        /// Parses an article document and returns the body HTML.
        /// </summary>
        /// <param name="doc">Root HTML document</param>
        /// <returns></returns>
        public abstract string GetBodyHtml(HtmlDocument doc);

        /// <summary>
        /// Parses an article document and returns the body text.
        /// </summary>
        /// <param name="doc">Root HTML document</param>
        /// <returns></returns>
        public abstract string GetBodyText(HtmlDocument doc);

        public class ArticleLink
        {
            public string Title { get; set; }
            public string Link { get; set; }
        }
    }
}