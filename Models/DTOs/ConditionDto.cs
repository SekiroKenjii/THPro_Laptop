using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class CreateConditionDto
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Category name is too long")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 200, ErrorMessage = "Decription for category is too long")]
        public string Description { get; set; }
    }
    public class UpdateConditionDto : CreateConditionDto { }
    public class ConditionDto : CreateConditionDto
    {
        public int Id { get; set; }
    }
}
