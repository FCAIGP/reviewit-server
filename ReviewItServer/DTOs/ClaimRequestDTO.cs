using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.DTOs
{
    public class ClaimRequestDTO
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string IdentificationCard { get; set; }
        [Required]
        public string ProofOfWork { get; set; }
        [Url]
        public string LinkedInAccount { get; set; }
        [Required]
        public string CompanyID { get; set; }
    }
}
