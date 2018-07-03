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

        /// <summary>
        /// Max parallelism used in <see cref="PreloadAllArticles"/>.
        /// </summary>
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

        /// <summary>
        /// Lists the articles available (including title and link) from the site.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Preloads the articles in parallel (see <see cref="MaxParalelism"/>) for future usage.
        /// </summary>
        /// <param name="articles">List of articles to preload.</param>
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

        /// <summary>
        /// Creates an article reference, with title and url.
        /// </summary>
        /// <param name="title">Title of the article.</param>
        /// <param name="url">Url of the article.</param>
        /// <returns>A new article instance.</returns>
        public Article CreateArticle(string title, string url)
        {
            CheckArticleLoader();

            return new Article(this, ArticleLoader) {Title = title, Url = url};
        }

        /// <summary>
        /// Returns the absolute URL of a link.
        /// </summary>
        /// <param name="baseUrl">Another URL that includes at least protocol and domain.</param>
        /// <param name="url">The relative (or full) URL.</param>
        /// <returns></returns>
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
