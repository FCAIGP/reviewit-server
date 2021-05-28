using ReviewItServer.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.Models
{
    public class Review
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ReviewId { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [EmailAddress]
        public string Contact { get; set; }
        [Range(0,1000000)]
        public int? Salary { get; set; }
        [StringLength(250)]
        public string JobDescription { get; set; }
        [StringLength(10000), DataType(DataType.MultilineText)]
        public string Body { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
        public string[] Tags { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        [Required, StringLength(50)]
        public string AuthorIP { get; set; }
        public virtual User Author { get; set; }
        public string AuthorId { get; set; }
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public Review()
        {
            Created = DateTime.Now;
        }
    }
}
