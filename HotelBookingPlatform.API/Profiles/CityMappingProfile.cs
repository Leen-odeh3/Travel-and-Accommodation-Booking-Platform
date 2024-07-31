﻿using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.API.Profiles;
public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, CityWithHotelsResponseDto>()
          .ForMember(dest => dest.Hotels, opt => opt.MapFrom(src => src.Hotels));

        CreateMap<CityCreateRequest, City>()
              .ForMember(dest => dest.CityImages,
                         opt => opt.MapFrom(src => src.CityImages != null
                             ? src.CityImages.Select(f => new Photo { FileName = f.FileName }).ToList()
                             : new List<Photo>()));
    }
}