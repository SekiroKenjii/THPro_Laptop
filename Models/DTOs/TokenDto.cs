using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class TokenDto
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
