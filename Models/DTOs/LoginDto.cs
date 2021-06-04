using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class LoginDto
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Username or Email is too long")]
        public string AccountName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        [Required]
        public bool IsUsingApp { get; set; }
    }
}
