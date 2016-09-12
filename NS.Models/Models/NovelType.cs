using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS.Models
{
    /// <summary>
    /// 小说分类
    /// </summary>
    public class NovelType
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string TypeName { get; set; }
        
        public DateTime AddTime { get; set; }
        
        [DefaultValue(false)]
        public bool IsDelete { get; set; }

        public virtual List<NovelTypeRelate> NovelTypeRelates { get; set; } 
    }
}
