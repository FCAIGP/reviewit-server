using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.ViewModels
{
    public class ReplyView
    {
        public string ReplyId { get; set; }
        public string ParentId { get; set; }
        public string AuthorId { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
}
