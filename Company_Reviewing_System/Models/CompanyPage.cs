using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class CompanyPage
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public string Headquarters { get; set; }
        public string Industry { get; set; }
        public string Region { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ClaimedDate { get; set; }
        public List<string> Subscribers { get; set; }
        public string LogoURL { get; set; }
        public bool PendingStatusChange { get; set; }
        public bool IsScoreUpToDate { get; set; }
        public double Score { get; set; }
        public List<Post> Posts { get; set; }
        public ClaimRequest AcceptedClaimRequest { get; set; }
        public List<ClaimRequest> ClaimRequestsHistory { get; set; }
        public CloseOptions CloseStatus { get; set; }
        public List<StatusChangeRequest> StatusHistory { get; set; }
        public List<Review> Reviews { get; set; }
        public User Owner { get; set; }
    }
}
