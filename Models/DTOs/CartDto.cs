using System;
using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class UpdateCartDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Count { get; set; }
    }
    public class CreateCartDto : UpdateCartDto
    {
        [Required]
        public Guid UserId { get; set; }
    }
    public class CartDto : CreateCartDto
    {
        public int Id { get; set; }
        public ProductDto Product { get; set; }
    }
}
