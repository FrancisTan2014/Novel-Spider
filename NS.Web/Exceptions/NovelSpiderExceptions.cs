using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS.Web.Exceptions
{
    /// <summary>
    /// 小说网站首页地址为空时抛出的异常信息
    /// </summary>
    public class NullIndexUrlException : Exception
    {
        public override string Message => "请提供小说网站首页地址";
    }

    /// <summary>
    /// 正则表达式有误或者找不到查找的内容时抛出的异常信息
    /// </summary>
    public class WrongPatternOrContentNotFoundException : Exception
    {
        public override string Message => "您提供的正则表达式有误，或者您提供的页面上并无您要查找的内容";
    }

    /// <summary>
    /// 正则表达式为空时抛出的异常信息
    /// </summary>
    public class NullPatternException : Exception
    {
        public override string Message => "请提供分析页面的正则表达式";
    }

    public class PatternExpiredException : Exception
    {
        public override string Message { get; }

        /// <summary>
        /// 以网站名称初始化本异常信息的实例
        /// </summary>
        /// <param name="siteName">网站名称</param>
        public PatternExpiredException(string siteName)
        {
            Message = $"{siteName}的正则表达式已过期，请尽快修复";
        }
    }
}
