using ReviewItServer.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.Models
{
    public class ClaimRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ClaimRequestId { get; set; }
        [Required, StringLength(200, MinimumLength = 20), DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required, StringLength(50, MinimumLength = 5)]
        public string Title { get; set; }
        [Required, StringLength(250)]
        public string IdentificationCard { get; set; } // Image
        [Required, StringLength(250)]
        public string ProofOfWork{ get; set; } // Image
        [Url]
        public string LinkedInAccount{ get; set; } // URL

        public virtual User Submitter { get; set; }
        
        public ClaimStatus ClaimStatus { get; set; }

    }
    public enum ClaimStatus
    {
        Approved,
        Rejected,
        Pending
    }
}
