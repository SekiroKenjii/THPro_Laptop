using System;

namespace Model.DTOs
{
    public class UpdateCartDto
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
    public class CreateCartDto : UpdateCartDto
    {
        public Guid UserId { get; set; }
    }
    public class CartDto : CreateCartDto
    {
        public int Id { get; set; }
        public ProductDto Product { get; set; }
    }
}
