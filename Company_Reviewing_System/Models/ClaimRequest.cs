using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
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
        public string? LinkedInAccount{ get; set; } // URL

        public virtual User Submitter { get; set; }
        
        public ClaimStatus ClaimStatus { get; set; }

        public static implicit operator ClaimRequestDto(ClaimRequest c)
        {
            return new ClaimRequestDto()
            {
                ClaimStatus = c.ClaimStatus,
                ClaimRequestId = c.ClaimRequestId,
                Description = c.Description,
                LinkedInAccount = c.LinkedInAccount,
                Title = c.Title,
                IdentificationCard = c.IdentificationCard,
                ProofOfWork = c.ProofOfWork
            };
        }

        public static ClaimRequest CreateFromDto(ClaimRequestDto request, User submitter)
        {
            ClaimRequest ret = new ClaimRequest
            {
                ClaimStatus = ClaimStatus.Pending,
                Description = request.Description,
                LinkedInAccount = request.LinkedInAccount,
                Title = request.Title,
                IdentificationCard = request.IdentificationCard,
                Submitter = submitter,
                ProofOfWork = request.ProofOfWork
            };

            return ret;
        }
    }
    public enum ClaimStatus
    {
        Approved,
        Rejected,
        Pending
    }
}
