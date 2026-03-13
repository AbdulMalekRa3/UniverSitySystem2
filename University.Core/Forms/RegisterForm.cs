using System.ComponentModel.DataAnnotations;

namespace University.Core.Forms
{
    public class RegisterForm
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
            ErrorMessage = "Password must be 8-15 characters and contain at least one uppercase, one lowercase, one number and one special character")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}