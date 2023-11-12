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
    public async Task<IEnumerable<CityDbModel>> GetCitiesAsync(string? name,string? searchQuery, bool includePointsOfInterest,int page, int limit)
    {
        var query = _cityInfoDbContext.Cities as IQueryable<CityDbModel>;
        if (includePointsOfInterest)
        {
            query = query.Include(c => c.PointsOfInterest);
        }
        if (!string.IsNullOrEmpty(name))
        { 
            name=name.Trim();
            query= query.Where(c => c.Name == name);
        }

        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            query = query.Where(c =>
                c.Name.Contains(searchQuery) || (c.Description != null && c.Description.Contains(searchQuery)));
        }

        int skip = (page - 1) * limit;
        var cities = await query
            .Skip(skip)
            .Take(limit)
            .OrderBy(c =>c.Name).ToListAsync();
        return cities;
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