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
        [Required, DataType(DataType.Date)]
        public DateTime Created { get; set; }
        [EmailAddress]
        public string? Contact { get; set; }
        [Range(0,1000000)]
        public int? Salary { get; set; }
        [StringLength(250)]
        public string? JobDescription { get; set; }
        [StringLength(10000)]
        public string Body { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public ICollection<Reply> Replies { get; set; }
        public string[] Tags { get; set; }
        public ICollection<Report> Reports { get; set; }
        [Required, StringLength(50)]
        public string AuthorIP { get; set; }
        public User? Author { get; set; }
    }
}
