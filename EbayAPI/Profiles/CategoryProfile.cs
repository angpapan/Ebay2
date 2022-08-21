using AutoMapper;
using EbayAPI.Dtos;
using EbayAPI.Models;
using NuGet.Protocol;

namespace EbayAPI.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}