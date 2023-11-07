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
        if(includePointsOfInterest)
            return await _cityInfoDbContext.Cities.Include(c => c.PointsOfInterest).FirstOrDefaultAsync(c => c.Id == cityId);
        return await  _cityInfoDbContext.Cities.FirstOrDefaultAsync(c => c.Id == cityId);
    }

    public async Task<IEnumerable<PointOfInterestDbModel>?> GetPointsOfInterestForCityAsync(int cityId)
    {
        var city = await _cityInfoDbContext.Cities.Include(c => c.PointsOfInterest).FirstOrDefaultAsync(c => c.Id == cityId);
        if(city == null) return null;
        return city.PointsOfInterest;
    }

    public async Task<PointOfInterestDbModel?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
    {
        if(!await CityExistsAsync(cityId))
            return null;
        return await _cityInfoDbContext.PointsOfInterest.FirstOrDefaultAsync(p => p.CityId == cityId && p.Id == pointOfInterestId);
    }
    
    public async Task<bool> CityExistsAsync(int cityId)
    {
        return await _cityInfoDbContext.Cities.AnyAsync(c => c.Id == cityId);
    }
    
    public async Task<PointOfInterestDbModel?> AddPointOfInterestAsync(int cityId, PointOfInterestDbModel pointOfInterest)
    {
        var city = await _cityInfoDbContext.Cities.Include(c => c.PointsOfInterest).FirstOrDefaultAsync(c => c.Id == cityId);
        if (city == null)
            return null;
        city.PointsOfInterest.Add(pointOfInterest);
        await _cityInfoDbContext.SaveChangesAsync();
        return pointOfInterest;
    }
    
    public async Task<bool> DeletePointOfInterest(int pointOfInterestId)
    {
        var pointOfInterest = await _cityInfoDbContext.PointsOfInterest.FirstOrDefaultAsync(p => p.Id == pointOfInterestId);
        if (pointOfInterest == null)
            return false;
        _cityInfoDbContext.PointsOfInterest.Remove(pointOfInterest);
        return await _cityInfoDbContext.SaveChangesAsync() ==1;
    }
    
    public async Task<bool> SaveChangesAsync()
    {
        // _cityInfoDbContext.ChangeTracker.DetectChanges();
        return await _cityInfoDbContext.SaveChangesAsync() >= 0;
    }
}