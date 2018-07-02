using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZohoKBScraper;

namespace Tests
{
    [TestClass]
    public class ScraperTests
    {
        private const string KbUrl = "https://deskportal.zoho.com/portal/engineerica/kb/tags/accudemia";

        [TestMethod]
        public void TestLoadLinks()
        {
            var scraper = new Scraper(KbUrl);
            var articles = scraper.GetArticles();

            Assert.IsTrue(articles.Count > 10, "articles not loaded");
        }
    }
}
