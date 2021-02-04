﻿using System;
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
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime? ClaimedDate { get; set; }
        public string[] SubscribersEmails { get; set; } // Email
        [StringLength(250)]
        public string? LogoURL { get; set; } // URL
        [Required]
        public bool PendingStatusChange { get; set; }
        [Required]
        public bool IsScoreUpToDate { get; set; }
        [Required]
        public double Score { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ClaimRequest? AcceptedClaimRequest { get; set; }
        public virtual ICollection<ClaimRequest> ClaimRequestsHistory { get; set; }
        public CloseOptions CloseStatus { get; set; }
        public virtual ICollection<StatusChangeRequest> StatusHistory { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        [InverseProperty("CurrentCompany")]
        public virtual ICollection<User> Employees { get; set; }
        public virtual User? Owner { get; set; }

        public static implicit operator CompanyPageDto(CompanyPage p)
        {
            return new CompanyPageDto()
            {
                CompanyId = p.CompanyId,
                Name = p.Name,
                Headquarters = p.Headquarters,
                Industry = p.Industry,
                CreatedDate = p.CreatedDate,
                Owner = p.Owner,
                LogoURL = p.LogoURL,
                Region = p.Region,
                Score = p.Score,
            };
        }
        public static CompanyPage CreateFrom(CompanyPageDto p)
        {
            return new CompanyPage()
            {
                Name = p.Name,
                Headquarters = p.Headquarters,
                Industry = p.Industry,
                CreatedDate = p.CreatedDate,
                Owner = p.Owner,
                LogoURL = p.LogoURL,
                Region = p.Region,
                Score = 0,
                PendingStatusChange = false,
                IsScoreUpToDate = true,
                CloseStatus = CloseOptions.Open,
                SubscribersEmails = Array.Empty<string>(),
            };
        }
    }
}
