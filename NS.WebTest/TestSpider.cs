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
            Action action = () => new Spider("");
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void TestGetTypes()
        {
            var spider = new Spider("http://www.biquge.tw/");
            spider.GetNovelTypes();

            spider.NovelTypesDic.Should().ContainKeys("玄幻奇幻", "武侠仙侠", "都市言情", "历史军事", "科幻灵异", "网游竞技", "女频频道");
            spider.NovelsDic.Should().ContainKeys("玄幻奇幻", "武侠仙侠", "都市言情", "历史军事", "科幻灵异", "网游竞技", "女频频道");
        }

        [TestMethod]
        public void TestGetNovels()
        {
            var spider = new Spider("http://www.biquge.tw/");
            spider.GetNovelTypes();
            spider.GetNovels();

            spider.NovelsDic["玄幻奇幻"].Should().Contain(new KeyValuePair<string, string>("武神血脉", "http://www.biquge.tw/9_9656/"));
            spider.NovelsDic["武侠仙侠"].Should().Contain(new KeyValuePair<string, string>("修神外传", "http://www.biquge.tw/0_443/"));
            spider.NovelsDic["都市言情"].Should().Contain(new KeyValuePair<string, string>("贴身兵王", "http://www.biquge.tw/7_7894/"));
            spider.NovelsDic["历史军事"].Should().Contain(new KeyValuePair<string, string>("抗日之将胆传奇", "http://www.biquge.tw/48_48248/"));
            spider.NovelsDic["科幻灵异"].Should().Contain(new KeyValuePair<string, string>("鬼妻", "http://www.biquge.tw/72_72844/"));
            spider.NovelsDic["网游竞技"].Should().Contain(new KeyValuePair<string, string>("无限西游", "http://www.biquge.tw/49_49822/"));
            spider.NovelsDic["女频频道"].Should().Contain(new KeyValuePair<string, string>("重生1994", "http://www.biquge.tw/21_21351/"));
        }
    }
}
