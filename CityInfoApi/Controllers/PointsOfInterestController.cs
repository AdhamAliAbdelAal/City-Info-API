using CityInfoApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoApi.Controllers;

[ApiController]
[Route("api/cities/{cityId}/[controller]")]
public class PointsOfInterestController : Controller
{
    [HttpGet]
    public ActionResult<PointsOfInterestResponseDto> GetPointsOfInterest(int cityId)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
            return NotFound();
        return Ok(new PointsOfInterestResponseDto(city.PointsOfInterest));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int id)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
            return NotFound();
        var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
        if (pointOfInterest == null)
            return NotFound();
        return Ok(pointOfInterest);
    }
}