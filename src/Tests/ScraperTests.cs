﻿using System;
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

            // Load requests
            _requester.RequestDelay = TimeSpan.FromMilliseconds(100);
            _scraper.PreloadAllArticles(articles);

            // Check everything was loaded
            Assert.IsTrue(articles.All(a => a.Loaded), "Not all articles were loaded");

            // Check parallelism
            Assert.AreEqual(Scraper.MaxParalelism, MockSiteRequester.MaxParallelRequests, "Parallelism not working");
        }
    }
}
