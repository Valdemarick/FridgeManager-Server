using Application.Models.FridgeModel;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class FridgeModelProfile : Profile
    {
        public FridgeModelProfile()
        {
            CreateMap<FridgeModel, FridgeModelDto>();
        }
    }
}