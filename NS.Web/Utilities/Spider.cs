using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace NS.Web.Utilities
{
    /// <summary>
    /// 爬虫类
    /// </summary>
    public class Spider
    {
        #region Properties
        public string BaseUrl { get; set; }

        /// <summary>
        /// 小说分类字典，键为名称值为地址
        /// </summary>
        public Dictionary<string, string> NovelTypesDic { get; set; }

        /// <summary>
        /// 小说名称及地址队列（结构：键：分类名称 - 值：（键：小说名称 - 值：小说目录地址））
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> NovelsDic { get; set; }

        public string NovelTypePattern => "<a\\s+href=\"(/newclass/\\d+/\\d+\\.html)\">([\\s\\S]+?)</a>";
        public string NovelNamePattern => "<a\\s+href=\"(/\\d+_\\d+/)\"[^>]*>([\\s\\S]+?)</a>";
        public string NovelContentPattern => "(<h1>[\\s\\S]+?</h1>)[\\s\\S]+?(<div\\sid=\"content\">[\\s\\S]+?</div>)";
        public string NovelAuthorPattern => "<h1>[\\s\\S]+?</h1>\\s*<p>\\s*作&nbsp;&nbsp;者：([^<]+)\\s*</p>";
        public string NovelChapterPattern => "<a(\\s*\\w+=\"[^\"]*\"\\s*href=\"/\\d+_\\d+.html\")";

        #endregion

        #region Events
        public event EventHandler<EventArgs> AfterGetNovelTypes;
        public event EventHandler<EventArgs> AfterGetNovels;
        #endregion

        #region Methods
        public Spider(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            BaseUrl = baseUrl;
            NovelTypesDic = new Dictionary<string, string>();
            NovelsDic = new Dictionary<string, Dictionary<string, string>>();
        }

        public void Start()
        {
            GetNovelTypes();
        }

        public void GetNovelTypes()
        {
            var indexHtml = HttpHelper.DownloadSource(BaseUrl);
            var regex = new Regex(NovelTypePattern);
            var matches = regex.Matches(indexHtml);

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    var name = match.Groups[2].Value;
                    var relateUrl = match.Groups[1].Value;
                    var abosoluteUrl = HttpHelper.RelateToAbsolute(BaseUrl, relateUrl);

                    NovelTypesDic.Add(name, abosoluteUrl);
                    NovelsDic.Add(name, new Dictionary<string, string>());
                }
            }

            AfterGetNovelTypes?.Invoke(this, EventArgs.Empty);
        }

        public void GetNovels()
        {
            var tasks = new Task[NovelTypesDic.Count];

            var count = 0;
            foreach (var typeName in NovelTypesDic.Keys)
            {
                var url = NovelTypesDic[typeName];
                var novelContainer = NovelsDic[typeName];

                tasks[count] = Task.Factory.StartNew(() =>
                {
                    var html = HttpHelper.DownloadSource(url);

                    var regex = new Regex(NovelNamePattern);
                    var matches = regex.Matches(html);
                    if (matches.Count > 0)
                    {
                        foreach (Match match in matches)
                        {
                            var name = match.Groups[2].Value;
                            var relateUrl = match.Groups[1].Value;
                            var abosoluteUrl = HttpHelper.RelateToAbsolute(url, relateUrl);

                            if (!novelContainer.ContainsKey(name))
                            {
                                novelContainer.Add(name, abosoluteUrl);
                            }
                        }
                    }
                });
                count++;
            }

            Task.WaitAll(tasks);

            AfterGetNovels?.Invoke(this, EventArgs.Empty);
        }

        public void GetChapters(string url)
        {
            var html = HttpHelper.DownloadSource(url);
        }

        #endregion
    }

    /// <summary>
    /// 获取章节成功后调用事件所传递的参数
    /// </summary>
    public class ChapterEventArgs : EventArgs
    {
        public string Author { get; set; }
        public Dictionary<string, string> ChapterLinksDic { get; set; }
    }
}