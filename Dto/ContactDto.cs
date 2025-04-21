using System.ComponentModel.DataAnnotations;

namespace ERE.DTO
{
    public class ContactDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string HomeNumber { get; set; }
        public string Street { get; set; }
        public string Village { get; set; }
        public string Commune { get; set; }
        public string District { get; set; }
        public string Province { get; set; }

    }
}