using AutoMapper;
using CityInfoApi.DTOs;
using CityInfoApi.Models;

namespace CityInfoApi.Profiles;

public class CityProfile : Profile
{
    public CityProfile()
    {
        this.CreateMap<CityDbModel, CityDto>(); 
    }
}