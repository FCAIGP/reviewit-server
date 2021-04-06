using ReviewItServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.ViewModels
{
    public class CompanyView
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public string Headquarters { get; set; }
        public string Industry { get; set; }
        public string Region { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LogoURL { get; set; }
        public bool IsScoreUpToDate { get; set; }
        public double Score { get; set; }
        public CloseOptions CloseStatus { get; set; }
        public string OwnerId { get; set; }
    }
}
