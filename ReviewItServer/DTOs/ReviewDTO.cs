using ReviewItServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.DTOs
{
    public class ReviewDTO
    {
        [EmailAddress]
        public string Contact { get; set; }
        public int? Salary { get; set; }
        public string JobDescription { get; set; }
        [Required]
        public string Body { get; set; }
        public string[] Tags { get; set; }
        [Required]
        public string CompanyId { get; set; }
        [Required]
        public bool IsAnonymous { get; set; }
    }
}
