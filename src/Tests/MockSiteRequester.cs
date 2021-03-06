﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using DocsScraper;

namespace Tests
{
    public class MockSiteRequester : ISiteRequester
    {
        private Dictionary<string, string> urls = new Dictionary<string, string>
        {
            {"https://deskportal.zoho.com/portal/engineerica/kb/tags/accudemia", "Tests.HtmlResults.article-list.html"},
            {"https://deskportal.zoho.com/portal/engineerica/kb/articles/what-do-i-do-if-the-accudemia-website-will-not-load", "Tests.HtmlResults.article.html"}
        };

        public TimeSpan? RequestDelay { get; set; }
        public static int CurrentParallelRequests;
        public static int MaxParallelRequests;
        private static readonly object _syncLock = new object();

        public string DefaultArticleResource;

        public string GetHtml(string url)
        {
            try
            {
                Interlocked.Increment(ref CurrentParallelRequests);
                if (RequestDelay != null)
                {
                    Thread.Sleep(RequestDelay.Value);
                }

                string file;
                if (urls.ContainsKey(url)) file = urls[url];
                else if (DefaultArticleResource != null)
                    file = DefaultArticleResource;
                else
                    throw new FileNotFoundException("Cannot request URL: " + url);

                return LoadResource(file);
            }
            finally
            {
                lock (_syncLock)
                {
                    if (CurrentParallelRequests > MaxParallelRequests)
                    {
                        MaxParallelRequests = CurrentParallelRequests;
                    }
                }
                Interlocked.Decrement(ref CurrentParallelRequests);
            }
        }

        private static string LoadResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
