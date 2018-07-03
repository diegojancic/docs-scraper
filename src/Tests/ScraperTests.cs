using System;
using System.Diagnostics;
using DocsScraper;
using DocsScraper.ZohoKb;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ScraperTests
    {
        private const string KbUrl = "https://deskportal.zoho.com/portal/engineerica/kb/tags/accudemia";
        private const string ArticleUrl = "https://deskportal.zoho.com/portal/engineerica/kb/articles/what-do-i-do-if-the-accudemia-website-will-not-load";
        private readonly Scraper _scraper;
        private readonly MockSiteRequester _requester;

        public ScraperTests()
        {
            _requester = new MockSiteRequester();
            _scraper = new Scraper(KbUrl, _requester, new ZohoArticleLoader());
        }

        [TestMethod]
        public void TestLoadLinks()
        {
            var articles = _scraper.GetArticles();
            
            Assert.AreEqual(20, articles.Count, "articles not loaded");
            Assert.AreEqual("Accudemia:  What should I do if the website will not load?", articles[0].Title);
            Assert.AreEqual(ArticleUrl, articles[0].Url);
        }

        [TestMethod]
        public void TestLoadArticle()
        {
            var article = _scraper.CreateArticle("Title", ArticleUrl);

            Assert.IsNotNull(article);
            Assert.IsFalse(article.Loaded);

            Assert.IsTrue(article.GetBodyHtml().StartsWith("<div>           In Accudemia this may happen if your network connection"), "HTML not loaded correctly");
            Assert.IsTrue(article.GetBodyText().StartsWith("In Accudemia this may happen if your network connection"), "Text body not loaded correctly");
            Assert.IsTrue(article.Loaded);
        }

        //[TestMethod]
        //public void TestLoadsArticlesInParallel()
        //{
        //    _requester.RequestDelay = TimeSpan.FromMilliseconds(200);

        //    var watch = new Stopwatch();
        //}
    }
}
