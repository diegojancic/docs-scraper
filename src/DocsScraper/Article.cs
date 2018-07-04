using System;
using System.Net;
using System.Runtime.CompilerServices;
using HtmlAgilityPack;

namespace DocsScraper
{
    /// <summary>
    /// Represents a documentation article, including title, URL, content and publication date.
    /// </summary>
    public class Article
    {
        private readonly Scraper _scraper;
        private readonly ArticleLoader _articleLoader;
        private HtmlDocument _htmlDocument;

        internal Article(Scraper scraper, ArticleLoader articleLoader)
        {
            _scraper = scraper;
            _articleLoader = articleLoader ?? throw new ArgumentNullException(nameof(articleLoader));
        }

        /// <summary>
        /// Title of the article. Not loaded from the article itself, but from the link.
        /// </summary>
        public string Title { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// Whether the article information was already loaded.
        /// </summary>
        public bool Loaded => _htmlDocument != null;

        /// <summary>
        /// Body of the article as HTML
        /// </summary>
        public string BodyHtml
        {
            get
            {
                if (!Loaded) LoadArticle();
                return _articleLoader.GetBodyHtml(_htmlDocument);
            }
        }

        /// <summary>
        /// Body of the article as text.
        /// </summary>
        public string BodyText
        {
            get
            {
                if (!Loaded) LoadArticle();
                return TextFormatter.Format(_articleLoader.GetBodyText(_htmlDocument).Trim());
            }
        }

        /// <summary>
        /// Returns the date and time the article was updated. Or null if that information could not be loaded.
        /// </summary>
        public DateTime? UpdatedDate
        {
            get
            {
                if (!Loaded) LoadArticle();
                return _articleLoader.GetUpdatedDate(_htmlDocument);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void LoadArticle()
        {
            if (Loaded) return;
            _htmlDocument = _scraper.LoadArticle(Url);
        }

        public override string ToString()
        {
            return $"{Title} ({Url})";
        }

        internal void Preload()
        {
            if (!Loaded) LoadArticle();
        }
    }
}