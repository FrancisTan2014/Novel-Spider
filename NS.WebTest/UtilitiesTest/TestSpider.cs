using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NS.Web.Utilities;

namespace NS.WebTest
{
    [TestClass]
    public class TestSpider
    {
        [TestMethod]
        public void TestCtor()
        {

        }

        [TestMethod]
        public void TestGetTypes()
        {
        }

        [TestMethod]
        public void TestGetNovels()
        {

        }

        [TestMethod]
        public void GetNovelInfoUrlsTest()
        {
            var spider = new Spider(new QuanshuAnalyzer());
            var types = spider.GetNovelTypes();
            var novels = spider.GetNovelUrls(types["玄幻魔法"]);

            novels.Should().HaveCount(665*30 - 8);
        }
    }
}
