using AutoMapper;
using CityInfoApi.DTOs;
using CityInfoApi.Models;
using CityInfoApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoApi.Controllers;

[ApiController]
[Route("api/cities/{cityId}/[controller]")]
[Authorize]
public class PointsOfInterestController : Controller
{
    
    private readonly ILogger<PointsOfInterestController> _logger;
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper  _mapper;
    public PointsOfInterestController(ILogger<PointsOfInterestController> logger,ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _logger = logger;
        _cityInfoRepository = cityInfoRepository;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest([FromRoute]int cityId)
    {
        var pointsOfInterest = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);
        if (pointsOfInterest == null)
            return NotFound();
        var pointsOfInterestDto = _mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterest);
        return Ok(pointsOfInterestDto);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest([FromRoute]int cityId, [FromRoute]int id)
    {
        var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, id);
        if (pointOfInterest == null)
            return NotFound();
        var pointOfInterestDto = _mapper.Map<PointOfInterestDto>(pointOfInterest);
        return Ok(pointOfInterestDto);
    }
    
    [HttpPost]
    public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest([FromRoute]int cityId, [FromBody]PointOfInterestRequestDto pointOfInterestRequestDto)
    {
        var pointOfInterestDbModel = _mapper.Map<PointOfInterestDbModel>(pointOfInterestRequestDto);
        var pointOfInterest = await _cityInfoRepository.AddPointOfInterestAsync(cityId, pointOfInterestDbModel);
        if (pointOfInterest==null)
        {
            NotFound();
        }
        var pointOfInterestDto = _mapper.Map<PointOfInterestDto>(pointOfInterest);
        return CreatedAtAction(nameof(GetPointOfInterest),
            new {cityId, id = pointOfInterest.Id}
            ,pointOfInterestDto);
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<PointOfInterestDto>> UpdatePointOfInterest([FromRoute]int cityId, [FromRoute]int id, [FromBody]PointOfInterestRequestDto pointOfInterest)
    {
        var pointOfInterestToUpdate = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, id);
        if (pointOfInterestToUpdate==null)
        {
            return NotFound();
        }
        _mapper.Map(pointOfInterest, pointOfInterestToUpdate);
        await _cityInfoRepository.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpPatch]
    [Route("{id}")]
    public async Task<ActionResult> PartiallyUpdatePointOfInterest([FromRoute]int cityId, [FromRoute]int id, [FromBody]JsonPatchDocument<PointOfInterestRequestDto> patchDocument)
    {
        var pointOfInterestToUpdate = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, id);
        if (pointOfInterestToUpdate==null)
        {
            return NotFound();
        }
        var pointOfInterestToPatch = _mapper.Map<PointOfInterestRequestDto>(pointOfInterestToUpdate);
        patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }
        if (!TryValidateModel(pointOfInterestToPatch))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(pointOfInterestToPatch, pointOfInterestToUpdate);
        await _cityInfoRepository.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeletePointOfInterest([FromRoute]int cityId, [FromRoute]int id)
    {
        if(!await _cityInfoRepository.CityExistsAsync(cityId))
            return NotFound();
        var pointOfInterestToDelete = await _cityInfoRepository.DeletePointOfInterest(id);
        if (!pointOfInterestToDelete)
        {
            return NotFound();
        }
        return NoContent();
    }
}