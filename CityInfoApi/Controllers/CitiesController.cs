using AutoMapper;
using CityInfoApi.DTOs;
using CityInfoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController: Controller
{
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;
    
    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetCities()
    {
        var cities = await _cityInfoRepository.GetCitiesAsync();
        var citiesDto = _mapper.Map<IEnumerable<CityDto>>(cities);
        return Ok(citiesDto);
    }
    
    [HttpGet]
    [Route("{id}")]
    public ActionResult<CityDto> GetCity([FromRoute]int id)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id); 
        if (city == null)
            return NotFound();
        return Ok(city);
    }
}