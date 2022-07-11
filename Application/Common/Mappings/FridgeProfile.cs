﻿using AutoMapper;
using Application.Models.Fridge;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class FridgeProfile : Profile
    {
        public FridgeProfile()
        {
            CreateMap<Fridge, FridgeDto>()
                .ForMember(dest => dest.Manufacturer,
                opt => opt.MapFrom(src => src.FridgeModel.Name))
                .ForMember(dest => dest.ProductionYear,
                opt => opt.MapFrom(src => src.FridgeModel.ProductionYear));

            CreateMap<FridgeForCreationDto, Fridge>();

            CreateMap<FridgeForUpdateDto, Fridge>().ReverseMap();
        }
    }
}