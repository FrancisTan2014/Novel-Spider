using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS.Models
{
    /// <summary>
    /// 小说实体
    /// </summary>
    public class Novel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Isdelete { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public int NovelTypeId { get; set; }
        public virtual List<NovelTypeRelate> NovelTypeRelates { get; set; }
        public virtual List<Chapter> Chapters { get; set; }
    }
}
