using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class StatusChangeRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RequestId { get; set; }
        [Required]
        public CloseOptions NewStatus { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        [Required, StringLength(1000)]
        public string Description { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
