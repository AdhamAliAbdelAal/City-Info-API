using AutoMapper;
using CityInfoApi.DTOs;
using CityInfoApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(policy:"MustBeAdham")]
public class CitiesController: Controller
{
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CitiesController> _logger;
    
    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper,ILogger<CitiesController>logger)
    {
        _cityInfoRepository = cityInfoRepository;
        _mapper = mapper;
        _logger = logger;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetCities([FromQuery]string?name, [FromQuery] string? searchQuery, [FromQuery] bool includePointsOfInterest=false, [FromQuery]int page = 1,[FromQuery]int limit = 10)
    {
        // _logger.LogInformation("Getting claims");
        foreach (var claim in User.Claims)
        {
            _logger.LogInformation($"Claim Type: {claim.Type} - Claim Value: {claim.Value}");
        }
        var cities = await _cityInfoRepository.GetCitiesAsync(name,searchQuery,includePointsOfInterest,page,limit);
        var citiesDto = _mapper.Map<IEnumerable<CityDto>>(cities);
        return Ok(citiesDto);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<CityDto>> GetCity([FromRoute]int id,[FromQuery]bool includePointsOfInterest = false)
    {
        var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
        if (city == null)
            return NotFound();
        var cityDto = _mapper.Map<CityDto>(city);
        return Ok(cityDto);
    }
}