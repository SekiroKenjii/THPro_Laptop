using Data.Enums;
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
    }

    public class UserInfo
    {
        public string GivenName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public string ProfilePicture { get; set; }
        public Gender Gender { get; set; }
        public bool LockoutEnabled { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
