using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class Reply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ReplyId { get; set; }
        [Required]
        public virtual Review Parent { get; set; }
        [Required, StringLength(1000)]
        public string Body { get; set; }
        [Required]
        public virtual User Author { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
