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
        [StringLength(250)]
        public string? CurrentJob { get; set; }
        public virtual CompanyPage? CurrentCompany { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateHired { get; set; }
        [StringLength(1000)]
        public string? Bio { get; set; }
        [StringLength(250)]
        public string? Image { get; set; } // Image
        public virtual ICollection<ClaimRequest> ClaimRequests { get; set; }
        public virtual ICollection<CompanyPage> OwnedCompanies{ get; set; }
        public virtual ICollection<Vote> Votes { get; set; }

    }
}
