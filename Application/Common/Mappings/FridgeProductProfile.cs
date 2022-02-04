using Application.Models.Fridge;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class ProductsInFridgeProfile : Profile
    {
        public ProductsInFridgeProfile()
        {
            CreateMap<FridgeProduct, FridgeProductDto>()
                .ForMember(dest => dest.ProductId,
                opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductCount,
                opt => opt.MapFrom(src => src.ProductQuantity));
        }
    }
}