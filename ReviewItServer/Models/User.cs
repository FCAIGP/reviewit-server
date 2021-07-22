using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.Models
{
    public class User: IdentityUser
    {

        [Required, StringLength(50)]
        public string FirstName { get; set; }
        [Required, StringLength(50)]
        public string LastName { get; set; }
        [StringLength(250)]
        public string CurrentJob { get; set; }
        public string CurrentCompanyCompanyId { get; set; }
        public virtual Company CurrentCompany { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateHired { get; set; }
        [StringLength(1000), DataType(DataType.MultilineText)]
        public string Bio { get; set; }
        [StringLength(250)]
        public string Image { get; set; } // Image
        public virtual ICollection<ClaimRequest> ClaimRequests { get; set; }
        public virtual ICollection<Company> OwnedCompanies{ get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
        
        [NotMapped]
        public string FullName { get => FirstName + " " + LastName; }
    }
}
