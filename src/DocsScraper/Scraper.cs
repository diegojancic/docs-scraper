using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace DocsScraper
{
    /// <summary>
    /// Loads the links and articles from a website
    /// </summary>
    public class Scraper
    {
        protected internal virtual string Url { get; set; }
        protected internal ArticleLoader ArticleLoader { get; set; }
        public const int MaxParalelism = 5;

        private readonly ISiteRequester _requester;

        /// <summary>
        /// Creates a new scraper for the Zoho KB.
        /// </summary>
        /// <param name="url">URL of the Zoho KB that contains a list of articles</param>
        /// <param name="requester">Class used to perform the HTTP requests.</param>
        /// <param name="articleLoader">Class responsible for parsing the HTML of the site</param>
        public Scraper(string url, ArticleLoader articleLoader, ISiteRequester requester = null)
        {
            Url = url;
            ArticleLoader = articleLoader;
            _requester = requester ?? new HttpSiteRequester();
        }

        public IList<Article> GetArticles()
        {
            CheckArticleLoader();

            var html = _requester.GetHtml(Url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var articles = new List<Article>();
            foreach (var artLink in ArticleLoader.GetLinks(doc))
            {
                // Load title and link
                string title = artLink.Title;
                string linkUrl = GetAbsoluteUrl(Url, artLink.Link);

                articles.Add(CreateArticle(title, linkUrl));
            }

            return articles;
        }

        public void PreloadAllArticles(IList<Article> articles)
        {
            Parallel.ForEach(articles,
                new ParallelOptions {MaxDegreeOfParallelism = MaxParalelism},
                a => a.Preload());
        }

        private void CheckArticleLoader()
        {
            if (ArticleLoader == null)
            {
                throw new Exception("ArticleLoader property has not been set.");
            }
        }

        public Article CreateArticle(string title, string url)
        {
            CheckArticleLoader();

            return new Article(this, ArticleLoader) {Title = title, Url = url};
        }

        public static string GetAbsoluteUrl(string baseUrl, string url)
        {
            // Get absolute URL
            if (url.StartsWith("/"))
            {
                var baseUri = new Uri(baseUrl);
                var relativeUri = new Uri(url, UriKind.RelativeOrAbsolute);
                var fullUri = new Uri(baseUri, relativeUri);
                return fullUri.ToString();
            }
            return url;
        }

        internal HtmlDocument LoadArticle(string url)
        {
            var html = _requester.GetHtml(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }
    }
}
