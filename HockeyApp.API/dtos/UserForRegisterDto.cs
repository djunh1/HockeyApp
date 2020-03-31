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
    }
}