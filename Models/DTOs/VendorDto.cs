using Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class CreateVendorDto
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Vendor name is too long")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Contact name is too long")]
        public string ContactName { get; set; }
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Contact title is too long")]
        public string ContactTitle { get; set; }
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Address name is too long")]
        public string Address { get; set; }
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "City name is too long")]
        public string City { get; set; }
        [StringLength(maximumLength: 20, ErrorMessage = "Country name is too long")]
        public string Country { get; set; }
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "Phone number is too long")]
        public string PhoneNumber { get; set; }
        [StringLength(maximumLength: 20, ErrorMessage = "Home page name is too long")]
        public string HomePage { get; set; }
        public Status Status { get; set; }
    }
    public class UpdateVendorDto : CreateVendorDto { }
    public class VendorDto : CreateVendorDto
    {
        public int Id { get; set; }
    }
}
