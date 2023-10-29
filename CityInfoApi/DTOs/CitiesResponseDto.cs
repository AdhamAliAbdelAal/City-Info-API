using System.Text.Json.Serialization;

namespace CityInfoApi.DTOs;

public class CitiesResponseDto
{
    [JsonPropertyName("cities")]
    public IEnumerable<CityDto> Cities { get; set; }
    
    public CitiesResponseDto(IEnumerable<CityDto> cities)
    {
        Cities = cities;
    }
}