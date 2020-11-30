using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class Post
    {
        public string PostId { get; set; }
        public CompanyPage Page{ get; set; }
        public User Author{ get; set; }
        public string Text { get; set; }
        public ICollection<string> Images{ get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
