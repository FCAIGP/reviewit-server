using ReviewItServer.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.Models
{
    public class Reply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ReplyId { get; set; }
        [Required]
        public virtual Review Parent { get; set; }
        [Required]
        public string ParentId { get; set; }
        [Required, StringLength(1000), DataType(DataType.MultilineText)]
        public string Body { get; set; }
        [Required]
        public string AuthorId {get; set;}
        [Required]
        public virtual User Author { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public Reply()
        {
            Date = DateTime.Now;
        }
    }
}
