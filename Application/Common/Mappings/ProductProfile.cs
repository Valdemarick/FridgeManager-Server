using Application.Models.Product;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<ProductForCreationDto, Product>();

            CreateMap<ProductForUpdateDto, Product>().ReverseMap();
        }
    }
}