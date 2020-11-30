using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class Review
    {
        public string ReviewId { get; set; }
        public DateTime Created { get; set; }
        public string? Contact { get; set; }
        public int? Salary { get; set; }
        public string? JobDescription { get; set; }
        public string Body { get; set; }
        public ICollection<User> Upvotes { get; set; }
        public ICollection<User> Downvotes { get; set; }
        public ICollection<Reply> Replies { get; set; }
        public ICollection<string> Tags { get; set; }
        public ICollection<Report> Reports { get; set; }
        public string AuthorIP { get; set; }
        public User? Author { get; set; }
    }
}
