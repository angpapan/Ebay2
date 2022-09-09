using AutoMapper;
using EbayAPI.Data;
using EbayAPI.Dtos;
using EbayAPI.Dtos.BidDtos;
using EbayAPI.Helpers;
using EbayAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EbayAPI.Services;

public class BidService
{
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    private readonly AppSettings _appSettings;
    private readonly EbayAPIDbContext _dbContext;
    private readonly IMapper _mapper;

    public BidService(IOptions<AppSettings> appSettings, EbayAPIDbContext dbContext, IMapper mapper)
    {
        _appSettings = appSettings.Value;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<int> SetNewBid(BidRequest bid, User bidder)
    {
        Item item = _dbContext.Items.Where(i => i.ItemId == bid.ItemId).SingleOrDefault();

        if (item == null)
             return -1;
        if (item.Ends < DateTime.Now)
            return -2;
        IQueryable<Bid> totalItemBids = _dbContext.Bids
            .Where(i => i.ItemId == bid.ItemId);

        decimal price;
        if (totalItemBids.IsNullOrEmpty())
            price = item.FirstBid;
        else
            price = totalItemBids.Select(i => i.Amount).Max();
        if (bid.Amount <=  price )
            return -3;

        Bid newBid = _mapper.Map<Bid>(bid);
        newBid.UserId = bidder.UserId;
        newBid.Time = DateTime.Now;
        
        List<int> bidids = _dbContext.Bids.Select(b => b.BidId).ToList();
        if (bidids.Count > 0) newBid.BidId = bidids.Max() + 1;
        else newBid.BidId = 1;
        
        _dbContext.Bids.Add(newBid);
        await _dbContext.SaveChangesAsync();
        return 1;
    }

    public async Task<List<Bid>> GetBids(User user)
    {
        return _dbContext.Bids.Where(i => i.UserId == user.UserId).ToList();
    }

    public async Task<PagedList<UserBidInfoDto>> GetUserBidInfo(BidderItemListQueryParameters dto, User user)
    {
        List<UserBidInfoDto> lista = new List<UserBidInfoDto>();

        List<int> items = await this.ItemsUserHaveBidded(user);

        IQueryable<UserBidInfoDto> itemq = _dbContext.Items
            .Include(i => i.Bids)
            .Include(i => i.Seller)
            .Include(i => i.Images)
            .Where(i=>items.Contains(i.ItemId))
            .Select(i => new UserBidInfoDto
            {
                MaxBid = i.Bids.Max(b => b.Amount),
                UserMaxBid = i.Bids.Where(b => b.UserId == user.UserId).Max(b => b.Amount),
                ItemId = i.ItemId,
                Name = i.Name,
                Description = i.Description,
                BuyPrice = i.BuyPrice,
                Ends = i.Ends,
                SellerUsername = i.Seller.Username,
                Image = i.Images != null && i.Images.Count > 0 ? Convert.ToBase64String(i.Images[0].ImageBytes) : null
            });
            
        if(dto.Search != null)
        {
            itemq = itemq.Where(i => i.Description.Contains(dto.Search) || i.Name.Contains(dto.Search));
        }
        
        PagedList<UserBidInfoDto> itemPage =
            PagedList<UserBidInfoDto>.ToPagedList(itemq, dto.PageNumber, dto.PageSize);

        return itemPage;
    }


    private async Task<List<int>> ItemsUserHaveBidded(User user)
    {
        return await _dbContext.Bids
            .Where(b => b.UserId == user.UserId)
            .Select(b => b.ItemId)
            .Distinct()
            .ToListAsync();
    }
    
    
}