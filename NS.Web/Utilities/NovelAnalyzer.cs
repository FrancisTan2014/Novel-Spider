using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Services.Description;
using NS.Utilities;
using NS.Web.Exceptions;

namespace NS.Web.Utilities
{
    /// <summary>
    /// 小说网站页面分析器基类，提供辅助爬虫程序分析小说网站的方法
    /// </summary>
    public abstract class NovelAnalyzer
    {
        #region Common Properties
        /// <summary>
        /// 小说网站首页地址
        /// </summary>
        public abstract string NovelSiteIndexUrl { get; set; }

        /// <summary>
        /// 小说网站名称
        /// </summary>
        public abstract string SiteName { get; set; }

        /// <summary>
        /// 本网站采用的编码方式
        /// </summary>
        public virtual Encoding Encode => Encoding.UTF8;
        #endregion

        #region Regular Expressions
        /// <summary>
        /// 查找小说分类及地址的正则表达式
        /// </summary>
        public abstract string NovelTypePattern { get; set; }

        /// <summary>
        /// 查找小说名称及地址的正则表达式
        /// </summary>
        public abstract string NovelNamePattern { get; set; }

        /// <summary>
        /// 查找小说作者的正则表达式
        /// </summary>
        public abstract string NovelAuthorPattern { get; set; }

        /// <summary>
        /// 查找小说描述信息的正则表达式
        /// </summary>
        public abstract string NovelDescPattern { get; set; }

        /// <summary>
        /// 查找小说封面图片地址的正则表达式
        /// </summary>
        public abstract string NovelCoverPattern { get; set; }

        /// <summary>
        /// 查找小说章节列表的正则表达式
        /// </summary>
        public abstract string NovelChaptersUrlPattern { get; set; }

        /// <summary>
        /// 查找小说章节名称及其地址的正则表达式
        /// </summary>
        public abstract string NovelChapterPattern { get; set; }

        /// <summary>
        /// 查找小说章节正文内容的正则表达式
        /// </summary>
        public abstract string NovelContentPattern { get; set; }

        /// <summary>
        /// 获取小说分类页面下方分页总页码的正则表达式
        /// </summary>
        public abstract string TotalPagePattern { get; set; }
        #endregion

        #region Methods for regular expressions
        /// <summary>
        /// 使用特定的正则表达式提取出小说分类名称及其地址的字典集
        /// </summary>
        public abstract Dictionary<string, string> GetNovelTypesDic(string html);

        /// <summary>
        /// 使用特定的正则表达式提取出小说名称及其地址的字典集
        /// </summary>
        public abstract Dictionary<string, string> GetNovelInfosDic(string html);

        /// <summary>
        /// 使用特定的正则表达式提取出小说作者名称
        /// </summary>
        public abstract string GetAuthor(string html);

        /// <summary>
        /// 使用特定的正则表达式提取出小说的描述信息
        /// </summary>
        public abstract string GetDescription(string html);

        /// <summary>
        /// 使用特定的正则表达式提取出小说封面图片地址
        /// </summary>
        public abstract string GetNovelCoverPath(string html);

        /// <summary>
        /// 使用特定的正则表达式提取出小说章节列表地址
        /// </summary>
        public abstract string GetChaptersUrl(string html);

        /// <summary>
        /// 使用特定的正则表达式提取出小说章节名称及其地址的字典集
        /// </summary>
        public abstract Dictionary<string, string> GetChaptersDic(string html);

        /// <summary>
        /// 使用特定的正则表达式提取出小说章节内容
        /// </summary>
        public abstract string GetChapterContent(string html);

        /// <summary>
        /// 使用特定的正则表达式提取出分页总页码
        /// </summary>
        public abstract int GetTotalPage(string html);

        #endregion

        #region Common methods
        /// <summary>
        /// 使用指定正则表达式匹配原始字符串，匹配成功后，根据指定的索引提取出键与值
        /// </summary>
        /// <param name="input">原始字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="keyIndex">指定匹配成功后从组中提取键的索引</param>
        /// <param name="valueIndex">指定匹配成功后从组中提取值的索引</param>
        /// <returns>从原始字符串中提取出来的键值对集合</returns>
        protected virtual Dictionary<string, string> GetDictionaryByPattern(string input, string pattern, int keyIndex,
            int valueIndex)
        {
            var dictionary = new Dictionary<string, string>();

            var matches = Regex.Matches(input, pattern);
            if (matches.Count == 0)
            {
                //throw new PatternExpiredException($"{SiteName}({NovelSiteIndexUrl})");
                return dictionary;
            }

            foreach (Match match in matches)
            {
                var key = match.Groups[keyIndex].Value;
                var value = match.Groups[valueIndex].Value;
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, value);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// 使用指定正则表达式匹配原始字符串，匹配成功后，根据指定的索引从组中提取出需要的值
        /// </summary>
        /// <param name="input">原始字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="index">提取组的索引</param>
        /// <returns>指定匹配值</returns>
        protected virtual string GetPointString(string input, string pattern, int index)
        {
            var match = Regex.Match(input, pattern);
            if (!match.Success)
            {
                //throw new PatternExpiredException($"{SiteName}({NovelSiteIndexUrl})");
                return string.Empty;
            }

            return match.Groups[index].Value;
        }

        /// <summary>
        /// 根据小说分类页面页码及其参考地址，构造出指定页码的url
        /// </summary>
        /// <param name="referenceUrl">参考地址</param>
        /// <param name="pageIndex">分页页码</param>
        /// <returns>指定页码的Url</returns>
        public virtual string BuildNovelTypePageUrl(string referenceUrl, int pageIndex)
        {
            var pattern = "([\\s\\S]+/[a-zA-Z0-9-_]*)(\\d+)(\\.(htm|html))";
            var newUrl = Regex.Replace(referenceUrl, pattern, match => match.Groups[1].Value + pageIndex + match.Groups[3].Value);

            return newUrl;
        }
        #endregion
    }
}