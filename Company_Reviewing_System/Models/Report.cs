using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class Report
    {
        public string ReportId { get; set; }
        public string AuthorIP { get; set; }
        public User? Author { get; set; }

        public DateTime Date {get; set;}
    }
    public enum ReportTypes
    {
       Spam,
       Inappropiate,
       Wrong
    }
}
