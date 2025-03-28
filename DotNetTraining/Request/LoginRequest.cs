using System.ComponentModel.DataAnnotations;

namespace DotNetTraining.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50, ErrorMessage = "Username must not exceed 50 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [MaxLength(100, ErrorMessage = "Password must not exceed 100 characters.")]
        public string Password { get; set; }

    }
}
