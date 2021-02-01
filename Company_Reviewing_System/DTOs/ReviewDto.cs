using Company_Reviewing_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class ReviewDto
    {
        public string? Contact { get; set; }
        public int? Salary { get; set; }
        public string? JobDescription { get; set; }
        public string Body { get; set; }
        public string[] Tags { get; set; }
        public string CompanyId { get; set; }
        [Display(Name ="Post anonymously")]
        public bool IsAnonymous { get; set; }
    }
}
