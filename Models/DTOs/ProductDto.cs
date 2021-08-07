using Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class CreateProductDto
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Product name is too long")]
        public string Name { get; set; }

        [Required]
        public int VendorId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int ConditionId { get; set; }

        [Required]
        public int DemandId { get; set; }

        [Required]
        public int TrademarkId { get; set; }

        [Required]
        public int UnitsInStock { get; set; }

        [Required]
        public bool Discontinued { get; set; }
    }
    public class UpdateProductDto : CreateProductDto 
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "CPU field is too long")]
        public string Cpu { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Screen field is too long")]
        public string Screen { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Ram field is too long")]
        public string Ram { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "GPU field is too long")]
        public string Gpu { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Storage field is too long")]
        public string Storage { get; set; }

        [Required]
        [StringLength(maximumLength: 10, ErrorMessage = "Pin field is too long")]
        public string Pin { get; set; }

        [Required]
        [StringLength(maximumLength: 200, ErrorMessage = "Connection field is too long")]
        public string Connection { get; set; }

        [Required]
        [StringLength(maximumLength: 10, ErrorMessage = "Weight field is too long")]
        public string Weight { get; set; }

        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "OS field is too long")]
        public string OS { get; set; }

        [Required]
        [StringLength(maximumLength: 10, ErrorMessage = "Color field is too long")]
        public string Color { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Warranty field is too long")]
        public string Warranty { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int UnitsOnOrder { get; set; }

        public string Description { get; set; }
    }
    public class ProductDto : UpdateProductDto
    {
        public int Id { get; set; }
        public IList<ProductImageDto> ProductImages { get; set; }
        public CategoryDto Category { get; set; }
        public ConditionDto Condition { get; set; }
        public DemandDto Demand { get; set; }
        public VendorDto Vendor { get; set; }
        public TrademarkDto Trademark { get; set; }
    }
}
