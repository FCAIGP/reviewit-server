using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.Models
{
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ReportId { get; set; }
        [Required, StringLength(50)]
        public string AuthorIP { get; set; }
        public virtual User Author { get; set; }
        [Required]
        public DateTime Date {get; set;}
    }
    public enum ReportTypes
    {
       Spam,
       Inappropiate,
       Wrong
    }
}
