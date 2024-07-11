using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using Core.Configuration;
using Core.Reports;
using final.Pages;
using NUnit.Framework;

namespace final.Test
{
    public class SearchBookTest:BaseTest
    {
        private SearchBookPage searchBookPage;
        [SetUp]
        public new void Setup()
        {
            searchBookPage = new();
            searchBookPage.GoToUrl(ConfigurationHelper.GetConfiguration()["searchBookURL"]);
        }

        [Test]
        [Category("SearchBook")]
        [TestCase("Design")]
        [TestCase("design")]
        public void SearchBook(string criteria)
        {
            ReportLog.Info("1. Search book " + criteria);
            searchBookPage.Search(criteria);
            ReportLog.Info("2. Verify search results");
            searchBookPage.VerifySearchResult(criteria);
        }
    }
}