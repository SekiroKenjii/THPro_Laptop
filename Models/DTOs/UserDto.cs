using Data.Enums;
using Microsoft.AspNetCore.Http;

namespace Model.DTOs
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string SubRole { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
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
    }
}
