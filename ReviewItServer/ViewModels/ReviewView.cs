using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.ViewModels
{
    public class ReviewView
    {
        public string ReviewId { get; set; }
        public DateTime Created { get; set; }
        public string Contact { get; set; }
        public int? Salary { get; set; }
        public string JobDescription { get; set; }
        public string Body { get; set; }
        public string[] Tags { get; set; }
        public string CompanyId { get; set; }
        public string AuthorId { get; set; }
    }
}
