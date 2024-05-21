using AutoMapper;
using backend.version1.Domain.DTO;
using Domain.Entities;

namespace backend.version1.Domain.AutoMapper.ProductProfile
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.FormattedPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}