using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS.Models
{
    /// <summary>
    /// 小说作者
    /// </summary>
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public virtual List<Novel> Novels { get; set; }
    }
}
