using System;
using System.Runtime.CompilerServices;
using HtmlAgilityPack;

namespace DocsScraper
{
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

        public string Title { get; set; }

        public string Url { get; set; }

        public bool Loaded => _htmlDocument != null;

        public string GetBodyHtml()
        {
            if (!Loaded) LoadArticle();
            return _articleLoader.GetBodyHtml(_htmlDocument);
        }

        public string GetBodyText()
        {
            if (!Loaded) LoadArticle();
            return _articleLoader.GetBodyText(_htmlDocument).Trim();
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