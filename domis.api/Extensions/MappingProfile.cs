using AutoMapper;
using domis.api.DTOs;
using domis.api.Models;

namespace domis.api.Extensions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDetailDto>()
            .ForMember(dest => dest.ImageUrls, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryPaths, opt => opt.Ignore());
    }
}