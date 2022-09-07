using AutoMapper;
using EbayAPI.Dtos;
using EbayAPI.Dtos.ImageDtos;
using EbayAPI.Models;

namespace EbayAPI.Profiles;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<Image, Base64WithIdImageDto>()
            .ForMember(dest => dest.Data,
                opt =>
                    opt.MapFrom(src => Convert.ToBase64String(src.ImageBytes)));
        
        CreateMap<Image, Base64ImageDto>()
            .ForMember(dest => dest.Data,
                opt =>
                    opt.MapFrom(src => Convert.ToBase64String(src.ImageBytes)));
        
    }
}