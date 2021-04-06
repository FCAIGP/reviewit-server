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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }
        public Company CurrentCompany { get; set; }
        public string CurrentJob { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateHired { get; set; }
        public string UserName { get; set; }
        public string FullName { get => FirstName + " " + LastName; }
        public string Image { get; set; }
        public string Email { get; set; }

        public static implicit operator UserDto(User u)
        {
            return new UserDto()
            {

                FirstName = u.FirstName,
                LastName = u.LastName,
                Bio = u.Bio,
                CurrentCompany = u.CurrentCompany,
                CurrentJob = u.CurrentJob,
                DateHired = u.DateHired,
                UserName = u.UserName,
                Image = u.Image,
                Email = u.Email,
            };
        }
    }
}
