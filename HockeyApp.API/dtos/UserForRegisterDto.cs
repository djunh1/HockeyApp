using System;
using System.ComponentModel.DataAnnotations;

namespace HockeyApp.API.dtos
{
    public class UserForRegisterDto
    {
        // Same name as database volumns

        // Perform the validation in a DTO
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 8 characters")]
        public string Password { get; set; }

        [Required]
        public string PlayerPosition { get; set; }
        [Required]
        public string KnownAs { get; set; }
        public DateTime LastActive { get; set; } // Testing datetime fields with this, delete later
        public DateTime Created { get; set; } // Testing datetime fields with this, delete later
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            // LastActive = DateTime.Now;
        }
    }
}