using System.Text.Json.Serialization;

namespace CityInfoApi.DTOs;

public class PointOfInterestDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}