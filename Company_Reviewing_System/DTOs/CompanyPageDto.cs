using Company_Reviewing_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class CompanyPageDto
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public string Headquarters { get; set; }
        public string Industry { get; set; }
        public string Region { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LogoURL { get; set; } // URL
        public double Score { get; set; }
        public User? Owner { get; set; }
    }
}
