using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NS.Web.Models;
using NS.Web.Utilities;

namespace NS.WebTest.ModelsTest
{
    [TestClass]
    public class NovelAnalyzerTest
    {
        [TestMethod]
        public void BuildNovelTypePageUrl()
        {
            var analyzer = new QuanshuAnalyzer();
            var url = analyzer.BuildNovelTypePageUrl("http://www.quanshu.net/list/1_2.html", 324);

            url.Should().Be("http://www.quanshu.net/list/1_324.html");
        }
    }
}
