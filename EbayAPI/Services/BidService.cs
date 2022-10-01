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

/// <summary>
/// Services for create a new bid and get all the bids that a user has make 
/// </summary>

public class BidService
{
    
    private readonly EbayAPIDbContext _dbContext;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Constructor of bid services
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="mapper"></param>
    public BidService( EbayAPIDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Create a new bid for an item
    /// </summary>
    /// <param name="bid">Item id and amount of bid </param>
    /// <param name="bidder">User that makes the bid </param>
    /// <exception cref="KeyNotFoundException">The item does'nt exists </exception>
    /// <exception cref="Exception">Auction has not started or has been ended,
    /// item already sold, low amount of bid or bidder is the seller </exception>

    public async Task SetNewBid(BidRequest bid, User? bidder)
    {
        if (bidder == null)
            throw new Exception("You must be logged in to make bids");
        
        Item? item = _dbContext.Items.Find(bid.ItemId);
        if (item == null)
            throw new BadHttpRequestException("No such item found.");

        if (item.Started == null)
            throw new Exception("Auction has not started yet");

        if (item.Ends < DateTime.Now )
            throw new Exception("Auction has been ended");

        if (item.BuyPrice != null && item.Price == item.BuyPrice)
            throw new Exception("Item has been already sold");
        
        if (item.Price > bid.Amount)
            throw new Exception("Offer must be greater than current bid!");

        if (item.SellerId == bidder.UserId)
            throw new Exception("You can not bid at your own items");
        
        
        
        Bid newBid = _mapper.Map<Bid>(bid);
        newBid.UserId = bidder.UserId;
        newBid.Time = DateTime.Now;
        
        _dbContext.Bids.Add(newBid);
        item.Price = bid.Amount;
        await _dbContext.SaveChangesAsync();
        
        


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