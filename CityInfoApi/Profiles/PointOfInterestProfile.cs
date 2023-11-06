using AutoMapper;
using CityInfoApi.DTOs;
using CityInfoApi.Models;

namespace CityInfoApi.Profiles;

public class PointOfInterestProfile : Profile
{
    public PointOfInterestProfile()
    {
        this.CreateMap<PointOfInterestDbModel, PointOfInterestDto>();
    }
}