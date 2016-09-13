using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using NS.Utilities;
using NS.Web.Exceptions;

namespace NS.Web.Utilities
{
    /// <summary>
    /// 全书小说网网页分析器(http://www.quanshu.net/)
    /// </summary>
    public class QuanshuAnalyzer : NovelAnalyzer
    {
        public sealed override string NovelSiteIndexUrl { get; set; }
        public sealed override string SiteName { get; set; }
        public override Encoding Encode => Encoding.GetEncoding("GBK");

        public sealed override string NovelTypePattern { get; set; }
        public sealed override string NovelNamePattern { get; set; }
        public sealed override string NovelAuthorPattern { get; set; }
        public sealed override string NovelDescPattern { get; set; }
        public sealed override string NovelCoverPattern { get; set; }
        public sealed override string NovelChaptersUrlPattern { get; set; }
        public sealed override string NovelChapterPattern { get; set; }
        public sealed override string NovelContentPattern { get; set; }
        public sealed override string TotalPagePattern { get; set; }

        public QuanshuAnalyzer()
        {
            SiteName = "全书网";
            NovelSiteIndexUrl = "http://www.quanshu.net/";
            NovelTypePattern = "<a\\s+href=\"(http://www\\.quanshu\\.net/list/\\d+_\\d+.html)\">([\\s\\S]+?)</a>";
            NovelNamePattern = "<a\\s+target=\"_blank\"\\s+title=\"[^\"]+\"\\s+href=\"([^\"]+)\"\\s+class=\"clearfix\\s+stitle\">([\\s\\S]+?)</a>";
            NovelAuthorPattern = "<dt>\\s*作\\s*者：\\s*</dt>\\s*<dd>\\s*<a[^>]+>([\\s\\S]+?)</a>\\s*</dd>";
            NovelDescPattern = "<div\\s+id=\"waa\\s*\"[^>]*>([\\s\\S]+?)</div>";
            NovelCoverPattern = "<div\\s+class=\"detail\">[\\s\\S]+?<img\\s+onerror=\"[^\"]+\"\\s*src=\"([^\"]+)\"[^>]+>";
            NovelChaptersUrlPattern = "<div\\s+class=\"b-oper\\s*\">\\s*<a\\s+href=\"([^\"]+)\"\\s*class=\"reader\\s*\"\\s*title=\"[^\"]+\"\\s*>";
            NovelChapterPattern = "<li>\\s*<a\\s+href=\"([^\"]+)\"\\s*title=\"[^\"]+\"\\s*>([\\s\\S]+?)</a>\\s*</li>";
            NovelContentPattern = "<div\\s+class=\"mainContenr\"\\s*id=\"content\">\\s*<script\\s*type=\"text/javascript\">[\\s\\S]+?</script>([\\s\\S]+?)<script\\s*type=\"text/javascript\">[\\s\\S]+?</script>\\s*</div>";
            TotalPagePattern = "<em\\s+id=\"pagestats\">\\d+/(\\d+)</em>";
        }

        public override Dictionary<string, string> GetNovelTypesDic(string html)
        {
            return GetDictionaryByPattern(html, NovelTypePattern, 2, 1);
        }

        public override Dictionary<string, string> GetNovelInfosDic(string html)
        {
            return GetDictionaryByPattern(html, NovelNamePattern, 2, 1);
        }

        public override string GetAuthor(string html)
        {
            return GetPointString(html, NovelAuthorPattern, 1);
        }

        public override string GetDescription(string html)
        {
            return GetPointString(html, NovelDescPattern, 1);
        }

        public override string GetNovelCoverPath(string html)
        {
            return GetPointString(html, NovelCoverPattern, 1);
        }

        public override string GetChaptersUrl(string html)
        {
            return GetPointString(html, NovelChaptersUrlPattern, 1);
        }

        public override Dictionary<string, string> GetChaptersDic(string html)
        {
            return GetDictionaryByPattern(html, NovelChapterPattern, 2, 1);
        }

        public override string GetChapterContent(string html)
        {
            return GetPointString(html, NovelContentPattern, 1);
        }

        public override int GetTotalPage(string html)
        {
            var total = GetPointString(html, TotalPagePattern, 1);
            return total.ToInt32();
        }
    }
}