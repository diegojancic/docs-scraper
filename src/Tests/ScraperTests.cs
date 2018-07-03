using System;
using System.Diagnostics;
using System.Linq;
using DocsScraper;
using DocsScraper.ZohoKb;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ScraperTests
    {
        private const string KbUrl = "https://deskportal.zoho.com/portal/engineerica/kb/tags/accudemia";
        private const string ArticleUrl = "https://deskportal.zoho.com/portal/engineerica/kb/articles/what-do-i-do-if-the-accudemia-website-will-not-load";
        private Scraper _scraper;
        private MockSiteRequester _requester;

        [SetUp]
        public void Setup()
        {
            _requester = new MockSiteRequester();
            _scraper = new Scraper(KbUrl, new ZohoArticleLoader(), _requester);
        }

        [Test]
        public void TestLoadLinks()
        {
            var articles = _scraper.GetArticles();
            
            Assert.AreEqual(20, articles.Count, "articles not loaded");
            Assert.AreEqual("Accudemia:  What should I do if the website will not load?", articles[0].Title);
            Assert.AreEqual(ArticleUrl, articles[0].Url);
        }

        [Test]
        public void TestLoadArticle()
        {
            var article = _scraper.CreateArticle("Title", ArticleUrl);

            Assert.IsNotNull(article);
            Assert.IsFalse(article.Loaded);

            Assert.IsTrue(article.BodyHtml.StartsWith("<div>           In Accudemia this may happen if your network connection"), "HTML not loaded correctly");
            Assert.IsTrue(article.BodyText.StartsWith("In Accudemia this may happen if your network connection"), "Text body not loaded correctly");
            Assert.IsTrue(article.Loaded);
        }

        [Test]
        public void TestLoadsArticlesInParallel()
        {
            _requester.DefaultArticleResource = "Tests.HtmlResults.article.html";
            var articles = _scraper.GetArticles();

            // Measure execution time
            var watch = Stopwatch.StartNew();
            _requester.RequestDelay = TimeSpan.FromMilliseconds(500);
            _scraper.PreloadAllArticles(articles);
            watch.Stop();

            // Check everything was loaded
            Assert.IsTrue(articles.All(a => a.Loaded), "Not all articles were loaded");

            // Check parallelism
            double expectedTime = _requester.RequestDelay.Value.TotalMilliseconds * articles.Count / Scraper.MaxParalelism;
            Console.WriteLine($"Expected complete in {expectedTime}ms ({expectedTime*.9}-{expectedTime * 1.1}). Completed in {watch.ElapsedMilliseconds}ms.");

            Assert.AreEqual(5, Scraper.MaxParalelism, "Parallelism not enabled by default");
            Assert.GreaterOrEqual(watch.ElapsedMilliseconds, expectedTime*.9, "Too much parallelism");
            Assert.LessOrEqual(watch.ElapsedMilliseconds, expectedTime*1.1, "Too little parallelism");

        }
    }
}
