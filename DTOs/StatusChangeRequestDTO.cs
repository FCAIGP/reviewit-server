using ReviewItServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.DTOs
{
    public class StatusChangeRequestDTO
    {
        [Required]
        public string CompanyId { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        [Required, StringLength(1000), DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        public CloseOptions NewStatus { get; set; }
    }
}
