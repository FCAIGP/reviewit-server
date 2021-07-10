using ReviewItServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.ViewModels
{
    public class StatusChangeRequestView
    {
        public string RequestId { get; set; }
        public CloseOptions NewStatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string CompanyId { get; set; }
    }
}
