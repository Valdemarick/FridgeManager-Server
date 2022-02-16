using Application.Models.User;
using AutoMapper;

namespace Application.Common.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}