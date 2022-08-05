using AutoMapper;
using EbayAPI.Dtos;
using EbayAPI.Models;
using NuGet.Protocol;

namespace Ebay.Profiles;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemDetails>();
        
        CreateMap<Item, ItemDetailsWithBids>();
        
    }
}
