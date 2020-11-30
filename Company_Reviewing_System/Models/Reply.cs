using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class Reply
    {
        public string ReplyId { get; set; }
        public Review Parent { get; set; }
        public string Body { get; set; }
        public User Author { get; set; }
        public DateTime Date { get; set; }
    }
}
