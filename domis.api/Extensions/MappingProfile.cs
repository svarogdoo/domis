using AutoMapper;
using domis.api.DTOs.Product;
using domis.api.Models;

namespace domis.api.Extensions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //CreateMap<Product, ProductDetailsDto>()
        //    .ForMember(dest => dest.Images, opt => opt.Ignore())
        //    .ForMember(dest => dest.CategoryPaths, opt => opt.Ignore());

        CreateMap<Product, ProductCompleteDetailsDto>()
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryPaths, opt => opt.Ignore())
            .ForMember(dest => dest.QuantityType, opt => opt.MapFrom(src => src.QuantityType));

    }
}