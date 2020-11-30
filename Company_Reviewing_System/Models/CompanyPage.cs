using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class CompanyPage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CompanyId { get; set; }
        [Required, StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }
        [Required, StringLength(50)]
        public string Headquarters { get; set; }
        [Required, StringLength(50)]
        public string Industry { get; set; }
        [Required, StringLength(50)]
        public string Region { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime ClaimedDate { get; set; }
        public string[] SubscribersEmails { get; set; } // Email
        [StringLength(250)]
        public string? LogoURL { get; set; } // URL
        [Required]
        public bool PendingStatusChange { get; set; }
        [Required]
        public bool IsScoreUpToDate { get; set; }
        [Required]
        public double Score { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ClaimRequest? AcceptedClaimRequest { get; set; }
        public ICollection<ClaimRequest> ClaimRequestsHistory { get; set; }
        public CloseOptions CloseStatus { get; set; }
        public ICollection<StatusChangeRequest> StatusHistory { get; set; }
        public ICollection<Review> Reviews { get; set; }
        [InverseProperty("CurrentCompany")]
        public ICollection<User> Employees { get; set; }
        public User? Owner { get; set; }
    }
}
