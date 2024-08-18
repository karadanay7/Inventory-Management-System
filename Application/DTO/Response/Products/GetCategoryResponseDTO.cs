using System;
using System.Text.Json.Serialization;
using Application.DTO.Request.Products;

namespace Application.DTO.Response.Products;

public class GetCategoryResponseDTO :UpdateCategoryRequestDTO
{
 [JsonIgnore]
    public virtual ICollection<GetProductResponseDTO> Products { get; set; } = null;
}
