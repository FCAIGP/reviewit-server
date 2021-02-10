using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class ClaimRequestDto
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string IdentificationCard { get; set; }
        public string ProofOfWork { get; set; }
        public string? LinkedInAccount { get; set; }
    }
}
