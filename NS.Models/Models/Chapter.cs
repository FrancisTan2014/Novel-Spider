using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS.Models
{
    /// <summary>
    /// 章节信息
    /// </summary>
    public class Chapter
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleWithNoSpace { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public int WordCount { get; set; }
        public int Sort { get; set; }
        public DateTime UpdateTime { get; set; }

        public int NovelId { get; set; }
        public virtual Novel Novel { get; set; }
    }
}
