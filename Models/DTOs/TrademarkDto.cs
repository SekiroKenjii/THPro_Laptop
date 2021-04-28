using Data.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class CreateTrademarkDto
    {
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "Trademark name is too long")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 256, ErrorMessage = "Trademark name is too long")]
        public string Description { get; set; }
        public Status Status { get; set; }

        public IFormFile Image { get; set; }
    }
    public class UpdateTrademarkDto : CreateTrademarkDto { }
    public class TrademarkDto : CreateTrademarkDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
    }
}
