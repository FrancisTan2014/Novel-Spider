using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NS.Web.Utilities
{
    /// <summary>
    /// Http请求工具类
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// 以string形式下载指定地址的资源
        /// </summary>
        /// <param name="url">指定下载地址</param>
        /// <param name="encoding">指定将下载资源的编码方式</param>
        /// <returns>下载到的string类型的资源</returns>
        public static string DownloadSource(string url, Encoding encoding = null)
        {
            var webClient = new WebClient { Encoding = encoding ?? Encoding.UTF8 };

            return webClient.DownloadString(url);
        }

        /// <summary>
        /// 以正则表达式提取给定url的主机地址，提取失败则返回原文
        /// </summary>
        /// <param name="url">给定url</param>
        /// <returns>给定Url的主机地址（末尾不带/）</returns>
        public static string GetHost(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var pattern = "((http|https|ftp)://([\\w-]+\\.)+[\\w-]+)[^\\s]*";
            var regexp = new Regex(pattern);

            if (regexp.IsMatch(url))
            {
                var match = regexp.Match(url);

                return match.Groups[1].Value;
            }

            return url;
        }

        /// <summary>
        /// 将页面上的相对地址处理为能访问的远程地址
        /// </summary>
        /// <param name="currentUrl">以http或者https开头当前所在页面地址</param>
        /// <param name="relateUrl">相对地址</param>
        /// <returns>能访问的远程地址</returns>
        public static string RelateToAbsolute(string currentUrl, string relateUrl)
        {
            if (string.IsNullOrEmpty(currentUrl))
            {
                throw new ArgumentNullException(nameof(currentUrl));
            }
            if (string.IsNullOrEmpty(relateUrl))
            {
                throw new ArgumentNullException(nameof(relateUrl));
            }
            if (!currentUrl.StartsWith("http://") && !currentUrl.StartsWith("https://"))
            {
                throw new ArgumentException("目录仅支持以http或者https协议访问的远程主机地址");
            }

            if (relateUrl.StartsWith("/"))
            {
                var host = GetHost(currentUrl);
                return host + relateUrl;
            }

            var lastIndex = currentUrl.LastIndexOf("/", StringComparison.Ordinal);
            if (lastIndex == 6 || lastIndex == 7)
            {
                return $"{currentUrl}/{relateUrl}";
            }

            var currentDir = currentUrl.Substring(0, lastIndex + 1);
            return currentDir + relateUrl;
        }
    }
}