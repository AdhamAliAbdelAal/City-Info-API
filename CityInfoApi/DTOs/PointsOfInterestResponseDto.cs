using System.Text.Json.Serialization;

namespace CityInfoApi.DTOs;

public class PointsOfInterestResponseDto
{
    [JsonPropertyName("pointsOfInterest")]
    public IEnumerable<PointOfInterestDto> PointsOfInterest { get; set; }
    
    public PointsOfInterestResponseDto(IEnumerable<PointOfInterestDto> pointsOfInterest)
    {
        PointsOfInterest = pointsOfInterest;
    }
}