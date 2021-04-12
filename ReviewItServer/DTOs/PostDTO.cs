using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.DTOs
{
    public class PostDTO
    {
        public string Text { get; set; }
        public string[] Images { get; set; }
        [Required]
        public string CompanyId { set; get; }

    }
}
