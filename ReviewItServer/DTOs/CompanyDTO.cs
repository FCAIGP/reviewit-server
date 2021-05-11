using System.ComponentModel.DataAnnotations;

namespace ReviewItServer.DTOs
{
    public class CompanyDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Headquarters { get; set; }
        [Required]
        public string Industry { get; set; }
        [Required]
        public string Region { get; set; }
        public string LogoURL { get; set; }
    }
}
