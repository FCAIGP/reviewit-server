using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PostId { get; set; }
        [Required]
        public CompanyPage Page{ get; set; }
        [Required]
        public User Author{ get; set; }
        [StringLength(10000)]
        public string? Text { get; set; }
        public string[] Images{ get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
    }
}
