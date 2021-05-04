using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class UpdateProductImageDto
    {
        public IList<IFormFile> Images { get; set; }
    }
    public class CreateProductImageDto : UpdateProductImageDto
    {
        [Required]
        public int ProductId { get; set; }
    }
    public class ProductImageDto : CreateProductImageDto
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public int SortOrder { get; set; }
    }
}
