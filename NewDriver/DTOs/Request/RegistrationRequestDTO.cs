using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace NewDriver.DTOs.Request
{
    public class RegistrationRequestDTO
    {
        

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; } = "Not Provided";

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? PhoneNumber { get; set; } = "+38900000000";

        [Required]
        [StringLength(13)]
        public string? EMBG { get; set; } = "0000000000000";

        public DateTime DOB { get; set; }

        public string? Password { get; set; }

        public string? Role { get; set; } = "Applicant";

        public string? Provimi { get; set; } = null;
        public string? Submitted { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; }
    }
}
