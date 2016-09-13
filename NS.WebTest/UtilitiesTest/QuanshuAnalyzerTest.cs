using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NS.Web.Utilities;

namespace NS.WebTest.UtilitiesTest
{
    [TestClass]
    public class QuanshuAnalyzerTest
    {
        [TestMethod]
        public void GetNovelCoverPathTest()
        {
            var analyzer = new QuanshuAnalyzer();
            var html = HttpHelper.DownloadSource("http://www.quanshu.net/book_295.html", analyzer.Encode);

            var cover = analyzer.GetNovelCoverPath(html);
            cover.Should().Be("http://img.quanshu.net/image/0/295/295s.jpg");
        }
    }
}
