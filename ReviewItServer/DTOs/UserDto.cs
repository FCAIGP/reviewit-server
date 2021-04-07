using ReviewItServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.DTOs
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string CurrentJob { get; set; }
        public string CurrentCompanyId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateHired { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
    }
}
