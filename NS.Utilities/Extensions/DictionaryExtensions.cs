using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS.Utilities.Extensions
{
    /// <summary>
    /// 对Dictionary类的扩展
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 将两个字典集合并
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Concat<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null) return;

            foreach (var key in target.Keys.Where(key => !source.ContainsKey(key)))
            {
                source.Add(key, target[key]);
            }
        }  
    }
}
