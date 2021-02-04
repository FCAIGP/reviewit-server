﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Models
{
    public class Review
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ReviewId { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [EmailAddress]
        public string? Contact { get; set; }
        [Range(0,1000000)]
        public int? Salary { get; set; }
        [StringLength(250)]
        public string? JobDescription { get; set; }
        [StringLength(10000), DataType(DataType.MultilineText)]
        public string Body { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
        public string[] Tags { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        [Required, StringLength(50)]
        public string AuthorIP { get; set; }
        public virtual User? Author { get; set; }
        public virtual CompanyPage Company { get; set; }

        public static Review CreateFromDto(ReviewDto dto, string ip, User? author)
        {
            Review ret = new Review
            {
                AuthorIP = ip,
                Created = DateTime.Now,
                Contact = dto.Contact,
                Salary = dto.Salary,
                JobDescription = dto.JobDescription,
                Body = dto.Body,
                Tags = dto.Tags,
                Author = author,
            };

            return ret;
        }
    }
}
