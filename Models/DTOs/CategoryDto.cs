using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Category name is too long")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Decription for category is too long")]
        public string Description { get; set; }
    }
    public class UpdateCategoryDto : CreateCategoryDto { }
    public class CategoryDto : CreateCategoryDto
    {
        public int Id { get; set; }
    }
}
