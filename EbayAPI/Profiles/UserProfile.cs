using AutoMapper;
using EbayAPI.Dtos;
using EbayAPI.Models;
using NuGet.Protocol;

namespace EbayAPI.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDetailsUser>()
            .ForMember(dest => dest.SellerRating,
                opt =>
                    opt.MapFrom(src => (src.SellerRatingsNum == 0) ? 0 : Math.Round(src.SellerRating / src.SellerRatingsNum, 2)))
            .ForMember(dest => dest.BidderRating,
                opt =>
                    opt.MapFrom(src => (src.BidderRatingsNum == 0) ? 0 : Math.Round(src.BidderRating / src.BidderRatingsNum, 2)));
        
        CreateMap<User, UserDetails>()
            .ForMember(dest => dest.SellerRating,
                opt =>
                    opt.MapFrom(src => (src.SellerRatingsNum == 0) ? 0 : Math.Round(src.SellerRating / src.SellerRatingsNum, 2)))
            .ForMember(dest => dest.BidderRating,
                opt =>
                    opt.MapFrom(src => (src.BidderRatingsNum == 0) ? 0 : Math.Round(src.BidderRating / src.BidderRatingsNum, 2)));

        CreateMap<UserDetailsUser, UserDetails>();
        
        CreateMap<User, UserReduced>();
        
        CreateMap<UserRegister, User>(MemberList.Source)
            .ForSourceMember(x => x.VerifyPassword, opt => opt.DoNotValidate());
    }
}