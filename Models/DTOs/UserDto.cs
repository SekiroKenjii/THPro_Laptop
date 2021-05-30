using Data.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Username is too long")]
        public string Username { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Email address is too long")]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "Phone number is too long")]
        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string SubRole { get; set; }

        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "First name is too long")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Last name is too long")]
        public string LastName { get; set; }

        [Required]
        [StringLength(maximumLength: 200, ErrorMessage = "Address name is too long")]
        public string Address { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "City name is too long")]
        public string City { get; set; }

        [StringLength(maximumLength: 20, ErrorMessage = "Country name is too long")]
        public string Country { get; set; }

        public IFormFile Image { get; set; }

        public Gender Gender { get; set; }
    }
    public class UpdateUserDto : CreateUserDto
    {
        public string NewPassword { get; set; }
    }
    public class UserDto : CreateUserDto
    {
        public string ProfilePicture { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}
