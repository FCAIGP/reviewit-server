﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PostId { get; set; }
        [ForeignKey(nameof(Company)), Column("PageCompanyId")]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Required]
        public virtual User Author { get; set; }
        [StringLength(10000), DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public string[] Images{ get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
