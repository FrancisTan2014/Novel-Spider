using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using NS.Models;
using NS.Web.Exceptions;
using NS.Utilities.Extensions;

namespace NS.Web.Utilities
{
    /// <summary>
    /// 小说网站爬虫类
    /// </summary>
    public class Spider
    {
        public NovelAnalyzer Analyzer { get; set; }

        private int _analysisTypePageTaskCount;
        /// <summary>
        /// 获取或设置在执行从小说列表页面提取小说名称及地址的过程时，允许同时开启的最大任务个数（不能超过20）
        /// </summary>
        public int AnalysisTypePageTaskCount
        {
            get { return _analysisTypePageTaskCount; ; }
            set
            {
                if (value <= 0)
                {
                    _analysisTypePageTaskCount = 5;
                }
                else if (value > 50)
                {
                    _analysisTypePageTaskCount = 20;
                }
                else
                {
                    _analysisTypePageTaskCount = value;
                }
            }
        }

        #region Methods
        public Spider(NovelAnalyzer analyzer)
        {
            if (analyzer == null)
            {
                throw new ArgumentNullException(nameof(analyzer), "请提供小说网页分析器");
            }

            Analyzer = analyzer;

            DefaultSettings();
        }

        private void DefaultSettings()
        {
            _analysisTypePageTaskCount = 20;
        }

        /// <summary>
        /// 使用分析器，执行对首页小说分类列表的提取过程，并返回分类名称及地址的字典集
        /// </summary>
        public Dictionary<string, string> GetNovelTypes()
        {
            if (string.IsNullOrEmpty(Analyzer.NovelSiteIndexUrl))
            {
                throw new NullIndexUrlException();
            }
            if (string.IsNullOrEmpty(Analyzer.NovelTypePattern))
            {
                throw new NullPatternException();
            }

            var indexHtml = HttpHelper.DownloadSource(Analyzer.NovelSiteIndexUrl, Analyzer.Encode);

            return Analyzer.GetNovelTypesDic(indexHtml);
        }

        /// <summary>
        /// 使用分析器，执行对指定小说分类页面的小说信息的提取过程，并返回小说名称及其地址的字典集
        /// </summary>
        /// <param name="typeUrl"></param>
        /// <param name="pageIndex">页码</param>
        public Dictionary<string, string> GetNovelUrls(string typeUrl, int pageIndex)
        {
            if (string.IsNullOrEmpty(typeUrl))
            {
                throw new ArgumentNullException(nameof(typeUrl));
            }
            if (string.IsNullOrEmpty(Analyzer.NovelNamePattern))
            {
                throw new NullPatternException();
            }

            typeUrl = HttpHelper.RelateToAbsolute(Analyzer.NovelSiteIndexUrl, typeUrl);
            
            var url = Analyzer.BuildNovelTypePageUrl(typeUrl, pageIndex);
            var pageHtml = HttpHelper.DownloadSource(url, Analyzer.Encode);

            return Analyzer.GetNovelInfosDic(pageHtml);
        }

        public int GetTotalPageCount(string referenceUrl)
        {
            referenceUrl = HttpHelper.RelateToAbsolute(Analyzer.NovelSiteIndexUrl, referenceUrl);
            var html = HttpHelper.DownloadSource(referenceUrl, Analyzer.Encode);

            // 获取最大页码
            var pageTotalCount = 1;
            if (!string.IsNullOrEmpty(Analyzer.TotalPagePattern))
            {
                pageTotalCount = Analyzer.GetTotalPage(html);
                if (pageTotalCount <= 0)
                {
                    pageTotalCount = 1;
                }
            }

            return pageTotalCount;
        }

        public Novel GetNovelInfo(string referenceUrl)
        {
            var html = HttpHelper.DownloadSource(referenceUrl, Analyzer.Encode);
            var author = Analyzer.GetAuthor(html);
            var desc = Analyzer.GetDescription(html);
            var cover = Analyzer.GetNovelCoverPath(html);

            var chaptersUrl = Analyzer.GetChaptersUrl(html);
            chaptersUrl = HttpHelper.RelateToAbsolute(referenceUrl, chaptersUrl);

            return new Novel
            {
                Author = new Author { Name = author },
                Description = desc,
                CoverUrl = cover,
                ChapterListUrl = chaptersUrl
            };
        }

        public Dictionary<string, string> GetChapterUrls(string chapterListUrl)
        {
            var html = HttpHelper.DownloadSource(chapterListUrl, Analyzer.Encode);
            return Analyzer.GetChaptersDic(html);
        }

        public string GetChapterContent(string chapterUrl)
        {
            var html = HttpHelper.DownloadSource(chapterUrl, Analyzer.Encode);
            return Analyzer.GetChapterContent(html);
        }
        #endregion
    }
}