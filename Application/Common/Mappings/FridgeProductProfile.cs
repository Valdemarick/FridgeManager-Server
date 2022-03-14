using Application.Models.Fridge;
using Application.Models.FridgeProduct;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class FridgeProductProfile : Profile
    {
        public FridgeProductProfile()
        {
            CreateMap<FridgeProduct, FridgeProductDto>()
                .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductCount,
                opt => opt.MapFrom(src => src.ProductQuantity)).ReverseMap();

            CreateMap<FridgeProductForCreationDto, FridgeProduct>().ReverseMap();
        }
    }
}