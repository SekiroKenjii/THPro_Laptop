using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class CreateDemandDto
    {
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "Demand name is too long")]
        public string Name { get; set; }
    }
    public class UpdateDemandDto : CreateDemandDto { }
    public class DemandDto : CreateDemandDto
    {
        public int Id { get; set; }
    }
}
