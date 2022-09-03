using AutoMapper;
using EbayAPI.Data;
using EbayAPI.Dtos;
using EbayAPI.Models;

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
    /// <exception cref="Exception">Auction has not started or has been ended, item already sold, low amount of bid or bidder is the seller </exception>

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

    /// <summary>
    /// get all bids that a user has make
    /// </summary>
    /// <param name="user">User that makes the request </param>
    /// <returns>A list with all bids of the user</returns>
    public async Task<List<Bid>> GetBids(User user)
    {
        return _dbContext.Bids.Where(i => i.UserId == user.UserId).ToList();
    }
    
    
}