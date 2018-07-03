# docs-scraper

This is a basic .NET (Framework, not tested under .NET Core) library to load documentation articles from the internet.

Currenly the only implementation is for the Zoho KB. Just enter the URL of the KB where a list of links are and it will download all the articles so you can put them in a database for local usage (eg, allow the user to search for them in your site).

Check the Tests for example usage.


Example Usage
-------------

To load the articles, do the following:
```cs
var url = "https://deskportal.zoho.com/portal/engineerica/kb/tags/accudemia";
var scraper = new Scraper(url, new ZohoArticleLoader();

// Load list of articles
var articles = scraper.GetArticles();

// Load all articles
// Optional but faster to load many articles in parallel.
scraper.PreloadAllArticles(articles);
```

Then, get the information for each article. For example:

```cs
articles[0].Title
articles[0].Url
articles[0].BodyHtml
articles[0].BodyText
```

License
-------

MIT License. Feel free to use anywhere, including commercial products. Created by Diego Jancic.