using System;
using System.Text.Json.Serialization;
using Application.DTO.Request.Products;

namespace Application.DTO.Response.Products;

public class GetLocationResponseDTO: UpdateLocationRequestDTO
{
    [JsonIgnore]
    public virtual ICollection<GetProductResponseDTO> Products { get; set; } = null;

}
