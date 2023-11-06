using CityInfoApi.Models;

namespace CityInfoApi.Services;

public interface ICityInfoRepository
{
    // the are two options
    // using IQueryable class to return a queryable object that may be further filtered with LINQ but this approach violates the repository pattern
    // using IEnumerable class to return a list and this approach is more in line with the repository pattern
    
    Task<IEnumerable<CityDbModel>> GetCitiesAsync();
    
    Task<CityDbModel?> GetCityAsync(int cityId, bool includePointsOfInterest);
    
    Task<IEnumerable<PointOfInterestDbModel>> GetPointsOfInterestForCityAsync(int cityId);
    
    Task<PointOfInterestDbModel?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
}