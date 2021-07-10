using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.ViewModels
{
    public class UserView
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CurrentJob { get; set; }
        public string CurrentCompanyCompanyId { get; set; }
        public DateTime? DateHired { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
    }
}
