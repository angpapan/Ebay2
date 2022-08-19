using AutoMapper;
using EbayAPI.Dtos;
using EbayAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace EbayAPI.Profiles;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemDetailsSimple>()
            .ForMember(dest=>dest.Images , 
                opt => opt.MapFrom(src => src.Images.Count > 0 ? src.Images[0] : null));


        CreateMap<Item, ItemDetails>()
            .ForMember(dest=>dest.Categories , opt => opt.MapFrom(
                src => src.ItemCategories.Select(c=>c.Category).Select(n=>n.Name).ToList()));

        CreateMap<Item, ItemDetailsFull>()
            .ForMember(dest=>dest.Categories , opt => opt.MapFrom(
                src => src.ItemCategories.Select(c=>c.Category).Select(n=>n.Name).ToList()));


        CreateMap<ItemDetails, ItemDetailsSimple>()
            .ForMember(dest=>dest.Images , 
            opt => opt.MapFrom(src => src.Images.Count > 0 ? src.Images[0] : null));

        CreateMap<ItemDetailsSimple, ItemDetails>();

        CreateMap<ItemAddition, Item>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.FirstBid))
            .ForSourceMember(src => src.CategoriesId, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.ImageFiles, opt => opt.DoNotValidate());


        CreateMap<Category, CategoryBasics>();

        CreateMap<BidRequest, Bid>()
            .ForMember(d => d.Time,
                o => o.MapFrom(s=>new DateTime()));

        CreateMap<Item, SellerItemListResponse>()
            .ForMember(dest => dest.HasBids,
                opt => opt.MapFrom(src => src.Bids!.Count > 0));
    }
}
