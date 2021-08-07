using System;
using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class CreateWishlistDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
    public class WishlistDto : CreateWishlistDto
    {
        public ProductDto Product { get; set; }
    }
}
