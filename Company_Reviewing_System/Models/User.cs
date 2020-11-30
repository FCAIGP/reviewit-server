using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class User: IdentityUser
    {

        [Required, StringLength(50)]
        public string FirstName { get; set; }
        [Required, StringLength(50)]
        public string LastName { get; set; }
        [Required, StringLength(250)]
        public string? CurrentJob { get; set; }
        [ForeignKey("CurrentCompany")]
        public CompanyPage? CurrentCompany { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateHired { get; set; }
        [StringLength(1000)]
        public string? Bio { get; set; }
        [Required, StringLength(250)]
        public string? Image { get; set; } // Image
        public ICollection<ClaimRequest> ClaimRequests { get; set; }
        public ICollection<CompanyPage> OwnedCompanies{ get; set; }
        public virtual ICollection<Vote> Votes { get; set; }

    }
}
