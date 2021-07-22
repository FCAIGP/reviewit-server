using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.ViewModels
{
    public class PostView
    {
        public string PostId { get; set; }
        public string Text { get; set; }
        public string[] Images { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CompanyId { get; set; }
    }
}
