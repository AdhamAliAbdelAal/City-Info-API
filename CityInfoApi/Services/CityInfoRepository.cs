using CityInfoApi.DbContexts;
using CityInfoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CityInfoApi.Services;

public class CityInfoRepository : ICityInfoRepository
{
    private readonly CityInfoDbContext _cityInfoDbContext;
    public CityInfoRepository( CityInfoDbContext cityInfoDbContext)
    {
        _cityInfoDbContext = cityInfoDbContext;
    }
    public async Task<IEnumerable<CityDbModel>> GetCitiesAsync()
    {
        var cities = _cityInfoDbContext.Cities.Include(c => c.PointsOfInterest).OrderBy(c => c.Name).ToListAsync();
        return await cities;
    }

    public async Task<CityDbModel?> GetCityAsync(int cityId, bool includePointsOfInterest)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<PointOfInterestDbModel>> GetPointsOfInterestForCityAsync(int cityId)
    {
        throw new NotImplementedException();
    }

    public async Task<PointOfInterestDbModel?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
    {
        throw new NotImplementedException();
    }
}