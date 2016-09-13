using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NS.Utilities
{
    /// <summary>
    /// 对string的扩展
    /// </summary>
    public static class StringExstension
    {
        /// <summary>
        /// 尝试将指定的原始字符串转换为Int32类型的值，若转换失败则返回给定的默认值
        /// </summary>
        /// <param name="source">原始字符串</param>
        /// <param name="def">转换失败时返回的值</param>
        /// <returns>由原始字符串转换而来的Int32类型的值或者指定的默认值</returns>
        public static int ToInt32(this string source, int def = default(int))
        {
            int result;
            return int.TryParse(source, out result) ? result : def;
        }

        /// <summary>
        /// 采用正则表达式去除指定文本中的html标记
        /// </summary>
        /// <param name="source">带有Html标记的原始文本</param>
        /// <returns>去除Html标记后的文本</returns>
        public static string RemoveHtmlTags(this string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var pattern = "(<[a-z]+>)|(</[a-z]+>)|(<br\\s*/\\s*>)|(\\s+)|(&nbsp;)";
            var regex = new Regex(pattern);

            return regex.Replace(source, "");
        }

        /// <summary>
        /// 采用正则表达式将原始文本中的标点符号替换为空，提取出纯文本并返回
        /// </summary>
        /// <param name="source">带有标点符号的原始文本</param>
        /// <returns>去除标点符号后的纯文本</returns>
        public static string RemovePunctuations(this string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            //var pattern = "[(\\pP)‘’“”]";
            //var regex = new Regex(pattern);

            //return regex.Replace(source, "");

            var stringBuilder = new StringBuilder();
            foreach (char c in source)
            {
                if (!char.IsPunctuation(c))
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 将原始字符串中的空白字符串全部移除，并返回新的字符串
        /// </summary>
        /// <param name="source">原始字符串</param>
        /// <returns>不带空白字符的新字符串</returns>
        public static string RemoveSpace(this string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Regex.Replace(source, "\\s", "");
        }

        /// <summary>
        /// 将原始字符串中的html标记、标点符号去除后，返回新字符串的字数（英文一个字符算一个）
        /// </summary>
        /// <param name="source">原始字符串</param>
        /// <returns>正文字数</returns>
        public static int ComputeWordCount(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return 0;
            }

            var noHtmlContent = source.RemoveHtmlTags();
            var noPunctuations = noHtmlContent.RemovePunctuations();

            return noPunctuations.Length;
        }
    }
}
