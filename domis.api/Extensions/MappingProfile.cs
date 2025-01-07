using AutoMapper;
using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Models.Enums;

namespace domis.api.Extensions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //CreateMap<Product, ProductDetailsDto>()
        //    .ForMember(dest => dest.Images, opt => opt.Ignore())
        //    .ForMember(dest => dest.CategoryPaths, opt => opt.Ignore());

        CreateMap<Product, Attributes>()
            .ForMember(dest => dest.QuantityType, opt => opt.MapFrom(src => src.QuantityType))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Depth, opt => opt.MapFrom(src => src.Depth))
            .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length))
            .ForMember(dest => dest.Thickness, opt => opt.MapFrom(src => src.Thickness))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight));


        CreateMap<Product, ProductDetailsDto>()
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryPaths, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.Ignore())
            .ForMember(dest => dest.Size, opt => opt.Ignore())
            .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => 
                string.IsNullOrEmpty(src.Title) ? src.Name : src.Title));;

            
        // CreateMap<Product, ProductDetailsDto>()
        //     .ForMember(dest => dest.Images, opt => opt.Ignore())
        //     .ForMember(dest => dest.CategoryPaths, opt => opt.Ignore())
        //     .ForMember(dest => dest.Price, opt => opt.Ignore())
        //     .ForMember(dest => dest.Size, opt => opt.Ignore())
        //     .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => new Attributes
        //     {
        //         QuantityType = src.QuantityType,
        //         Title = src.Title,
        //         Width = src.Width,
        //         Height = src.Height,
        //         Depth = src.Depth,
        //         Length = src.Length,
        //         Thickness = src.Thickness,
        //         Weight = src.Weight
        //     }));
            //.ForMember(dest => dest.QuantityType, opt => opt.MapFrom(src =>
            //    (ProductQuantityType)Enum.ToObject(typeof(ProductQuantityType), src.QuantityType)));

    }
}