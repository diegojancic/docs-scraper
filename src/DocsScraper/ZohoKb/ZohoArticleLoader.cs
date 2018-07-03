using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace DocsScraper.ZohoKb
{
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

        private static HtmlNode GetAnswerBody(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectSingleNode("//div[@id='answerContainer']");
        }
    }
}