using AutoMapper;
using backend.version1.Domain.DTO;
using backend.version1.Domain.Entities;

namespace backend.version1.Domain.AutoMapper.PostProfile
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            CreateMap<Post, PostDto>();
        }
    }
}