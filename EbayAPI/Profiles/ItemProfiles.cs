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
            .ForSourceMember(
                s => s.Bids,
                o => o.DoNotValidate())
            .ForSourceMember(
                s => s.ItemCategories, o => o.DoNotValidate())
            .ForSourceMember(
                s => s.VisitedByUsers, o => o.DoNotValidate())
            ;

        CreateMap<Item, ItemDetailsFull>()
            .ForMember(d=>d.Categories, 
                o=>o.MapFrom(s=>s.ItemCategories.Select(i=>i.Category).ToList()));


        CreateMap<ItemDetails, ItemDetailsSimple>()
            .ForMember(dest=>dest.Images , 
            opt => opt.MapFrom(src => src.Images.Count > 0 ? src.Images[0] : null));

        CreateMap<ItemDetailsSimple, ItemDetails>();

        CreateMap<ItemAddition, Item>()
            .ForSourceMember(src => src.CategoriesId, opt => opt.DoNotValidate());


        CreateMap<Category, CategoryBasics>();

        CreateMap<BidRequest, Bid>()
            .ForMember(d => d.Time,
                o => o.MapFrom(s=>new DateTime()));
    }
}
