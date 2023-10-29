using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CityInfoApi.DTOs;

public class PointOfInterestRequestDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(50)]
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

