using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace DocsScraper.ZohoKb
{
    /// <summary>
    /// Parses and loads the articles from the Zoho KB.
    /// </summary>
    public class ZohoArticleLoader : ArticleLoader
    {
        public override IEnumerable<ArticleLink> GetLinks(HtmlDocument doc)
        {
            var nodes = doc.DocumentNode.SelectNodes("//ol[@id='topicbyarticlelist']//a[@class='solutionlist']");

            foreach (var node in nodes)
            {
                string title = node.InnerText.Trim();
                var linkUrl = new StringBuilder(node.GetAttributeValue("href", null));
                linkUrl.Replace("&#x2f;", "/");

                yield return new ArticleLink
                {
                    Title = title,
                    Link = linkUrl.ToString()
                };
            }
        }

        public override string GetBodyHtml(HtmlDocument doc)
        {
            var body = GetAnswerBody(doc);
            return body.InnerHtml;
        }

        public override string GetBodyText(HtmlDocument doc)
        {
            var body = GetAnswerBody(doc);
            return body.InnerText;
        }

        private static readonly Regex DateRegex = new Regex(@"(?<date>[0-9]{2}\s[A-Za-z]{3}\s[0-9]{4}\s[0-9]{2}:[0-9]{2}\s[AP]M)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        public override DateTime? GetUpdatedDate(HtmlDocument doc)
        {
            try
            {
                var text = doc.DocumentNode.SelectSingleNode("//div[starts-with(@class, 'solupdatemsg')]").InnerText;
                // Format: "Updated: 12 Jun 2018 02:12 PM" but it includes several spaces in the middle

                var match = DateRegex.Match(text);
                return DateTime.ParseExact(match.Groups["date"].Value, "dd MMM yyyy hh:mm tt", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static HtmlNode GetAnswerBody(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectSingleNode("//div[@id='answerContainer']");
        }
    }
}