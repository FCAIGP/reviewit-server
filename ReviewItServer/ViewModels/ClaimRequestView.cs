using ReviewItServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.ViewModels
{
    public class ClaimRequestView
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string IdentificationCard { get; set; }
        public string ProofOfWork { get; set; }
        public string LinkedInAccount { get; set; }
        public ClaimStatus ClaimStatus { get; set; }

    }
}
