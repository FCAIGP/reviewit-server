using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.DTOs
{
    public class ReplyDTO
    {
        [Required]
        public string ParentId { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
