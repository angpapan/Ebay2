using AutoMapper;
using EbayAPI.Data;
using EbayAPI.Dtos;
using EbayAPI.Helpers;
using EbayAPI.Models;
using Microsoft.AspNetCore.Mvc;
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
    
    
}