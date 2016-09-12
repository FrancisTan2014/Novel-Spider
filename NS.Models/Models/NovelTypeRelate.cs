using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS.Models
{
    /// <summary>
    /// 小说类型与小说关联表
    /// </summary>
    public class NovelTypeRelate
    {
        public int Id { get; set; }
        public int NovelTypeId { get; set; }
        public int NovelId { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual NovelType NovelType { get; set; }
        public virtual Novel Novel { get; set; }
    }
}
