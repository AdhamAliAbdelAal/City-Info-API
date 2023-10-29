using CityInfoApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController: Controller
{
    [HttpGet]
    public ActionResult<CitiesResponseDto> GetCities()
    {
        return Ok(new CitiesResponseDto(CitiesDataStore.Current.Cities));
    }
    
    [HttpGet]
    [Route("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id); 
        if (city == null)
            return NotFound();
        return Ok(city);
    }
}