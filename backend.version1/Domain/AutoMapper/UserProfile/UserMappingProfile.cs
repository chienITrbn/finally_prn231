using AutoMapper;
using backend.version1.Domain.DTO;
using backend.version1.Domain.Entities;

namespace backend.version1.Domain.AutoMapper.UserProfile
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}