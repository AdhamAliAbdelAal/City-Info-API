using CityInfoApi.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoApi.Controllers;

[ApiController]
[Route("api/cities/{cityId}/[controller]")]
public class PointsOfInterestController : Controller
{
    [HttpGet]
    public ActionResult<PointsOfInterestResponseDto> GetPointsOfInterest([FromRoute]int cityId)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
            return NotFound();
        return Ok(new PointsOfInterestResponseDto(city.PointsOfInterest));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<PointOfInterestDto> GetPointOfInterest([FromRoute]int cityId, [FromRoute]int id)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
            return NotFound();
        var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
        if (pointOfInterest == null)
            return NotFound();
        return Ok(pointOfInterest);
    }
    
    [HttpPost]
    public ActionResult<PointOfInterestRequestDto> CreatePointOfInterest([FromRoute]int cityId, [FromBody]PointOfInterestRequestDto pointOfInterest)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
            return NotFound();
        var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
        var newPointOfInterest = new PointOfInterestDto
        {
            Id = ++maxPointOfInterestId,
            Name = pointOfInterest.Name,
            Description = pointOfInterest.Description
        };
        city.PointsOfInterest.Add(newPointOfInterest);
        return CreatedAtAction(nameof(GetPointOfInterest), new {cityId, id = newPointOfInterest.Id}, newPointOfInterest);
    }
    
    [HttpPut]
    [Route("{id}")]
    public ActionResult<PointOfInterestDto> UpdatePointOfInterest([FromRoute]int cityId, [FromRoute]int id, [FromBody]PointOfInterestRequestDto pointOfInterest)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
            return NotFound();
        var pointOfInterestToUpdate = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
        if (pointOfInterestToUpdate == null)
            return NotFound();
        pointOfInterestToUpdate.Name = pointOfInterest.Name;
        pointOfInterestToUpdate.Description = pointOfInterest.Description;
        return NoContent();
        //or return Ok(pointOfInterestToUpdate);
        // the two options are valid
    }
    
    [HttpPatch]
    [Route("{id}")]
    public ActionResult PartiallyUpdatePointOfInterest([FromRoute]int cityId, [FromRoute]int id, [FromBody]JsonPatchDocument<PointOfInterestRequestDto> patchDocument)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
            return NotFound();
        var pointOfInterestToUpdate = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
        if (pointOfInterestToUpdate == null)
            return NotFound();
        var pointOfInterestToPatch = new PointOfInterestRequestDto
        {
            Name = pointOfInterestToUpdate.Name,
            Description = pointOfInterestToUpdate.Description
        };
        patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!TryValidateModel(pointOfInterestToPatch))
            return BadRequest(ModelState);
        pointOfInterestToUpdate.Name = pointOfInterestToPatch.Name;
        pointOfInterestToUpdate.Description = pointOfInterestToPatch.Description;
        return NoContent();
        //or return Ok(pointOfInterestToUpdate);
        // the two options are valid
    }
    
    [HttpDelete]
    [Route("{id}")]
    public ActionResult DeletePointOfInterest([FromRoute]int cityId, [FromRoute]int id)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
            return NotFound();
        var pointOfInterestToDelete = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
        if (pointOfInterestToDelete == null)
            return NotFound();
        city.PointsOfInterest.Remove(pointOfInterestToDelete);
        return NoContent();
    }
}