using System;
using System.ComponentModel.DataAnnotations;
using NetcodeHub.Packages.Extensions.Attributes.RequiredGuid;

namespace Application.DTO.Response.Products;

public class ProductBaseDTO
{
    [Required]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Serial Number")]
    public string SerialNumber { get; set; }
    [NetcodeHubRequiredGuid(ErrorMessage ="The Product Location is required")]
    [RegularExpression(@"^[{(]?[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}[)}]?$", ErrorMessage = "The Product Location must be a valid GUID")]
    public Guid LocationId { get; set; }

    [NetcodeHubRequiredGuid(ErrorMessage = "The Product Category is required")]
    [RegularExpression(@"^[{(]?[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}[)}]?$", ErrorMessage = "The Product Category must be a valid GUID")]
    public Guid CategoryId { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    [Required]

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    [Required]
    [MinLength(10)]
    [MaxLength(5000)]
    public string Description { get; set; }
    [Required]
    [Display(Name = "Product Image")]
    public string Base64Image { get; set; }

}
