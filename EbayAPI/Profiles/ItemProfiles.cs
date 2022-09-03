using AutoMapper;
using EbayAPI.Dtos;
using EbayAPI.Dtos.BidsDtos;
using EbayAPI.Dtos.ImageDtos;
using EbayAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace EbayAPI.Profiles;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemDetailsSimple>()
            .ForMember(dest=>dest.Image , 
                opt => opt.MapFrom(
                    src => src.Images.Count > 0 ? Convert.ToBase64String(src.Images[0].ImageBytes) : null));


        CreateMap<Item, ItemDetails>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(
                src => src.ItemCategories.Select(c => c.Category).Select(n => n.Name).ToList()))
            .ForMember(dest => dest.SellerName, opt => opt.MapFrom(
                src => src.Seller.Username))
            .ForMember(d=>d.Images, o=>o.Ignore())
            ;

        CreateMap<Item, ItemDetailsFull>()
            .ForMember(dest=>dest.Categories , opt => opt.MapFrom(
                src => src.ItemCategories.Select(c=>c.Category).Select(n=>n.Name).ToList()))
            .ForMember(dest => dest.SellerName, opt => opt.MapFrom(
                src => src.Seller.Username))
            .ForMember(dest=>dest.Images, 
                opt=>opt.MapFrom(item => item.Images ))
            .ForMember(dest=>dest.Bids,o=>o.MapFrom(item => item.Bids))
            ;


        CreateMap<ItemDetails, ItemDetailsSimple>()
            .ForMember(dest=>dest.Image , 
            opt => opt.MapFrom(src => src.Images.Count > 0 ? src.Images[0] : null));

        CreateMap<ItemDetailsSimple, ItemDetails>();

        CreateMap<ItemAddition, Item>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.FirstBid))
            .ForSourceMember(src => src.CategoriesId, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.ImageFiles, opt => opt.DoNotValidate());


        CreateMap<Item, SellerItemListResponse>()
            .ForMember(dest => dest.HasBids,
                opt => opt.MapFrom(src => src.Bids!.Count > 0))
            .ForMember(dest => dest.Image,
                opt => opt.MapFrom(src =>
                    (src.Images != null && src.Images.Count > 0)
                        ? Convert.ToBase64String(src.Images[0].ImageBytes)
                        : null));

        CreateMap<Item, ItemToEditResponseDto>()
            .ForMember(dest => dest.CurrentImages,
                opt => opt.MapFrom(src =>
                    src.Images))
            .ForMember(dest => dest.AddedCategories,
                opt => 
                    opt.MapFrom(src => src.ItemCategories.Select(ic => ic.Category).ToList()));
        
        
        CreateMap<Category, CategoryBasics>();

        CreateMap<BidRequest, Bid>()
            .ForMember(d => d.Time,
                o => o.MapFrom(s=>new DateTime()));

        CreateMap<Bid, BidsForItemDetails>()
            .ForMember(d=>d.Bidder,
                o=>o.MapFrom(bid => bid.Bidder ))
            ;
        
        

    }
}
