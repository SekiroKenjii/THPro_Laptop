using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class CreateRoleDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 200, ErrorMessage = "Role description is too long")]
        public string Description { get; set; }
    }
    public class UpdateRoleDto : CreateRoleDto { }
    public class RoleDto : CreateRoleDto { }
}
