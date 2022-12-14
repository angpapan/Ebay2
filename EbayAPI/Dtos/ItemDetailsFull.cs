using EbayAPI.Dtos.BidsDtos;
using EbayAPI.Dtos.ImageDtos;
using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class ItemDetailsFull
{
    
    [Required] public int ItemId { get; set; }
    [Required] public string Name { get; set; }
    public decimal? BuyPrice {get; set;} = null;
    [Required] public decimal FirstBid {get; set;}
    [Required] public decimal Price { get; set; }
    public string Location { get; set; }
    public string? Country { get; set; } = null;
    [Required] public string Description { get; set; }
    public DateTime? Started {get; set;} = null;
    [Required] public DateTime Ends {get; set;}
    public decimal? Latitude { get; set; } = null;
    public decimal? Longitude { get; set; } = null;
    [Required] public int SellerId { get; set; }
    [Required] public string SellerName { get; set; }
    public List<string>? Categories {get; set;} = null;
    public List<Base64ImageDto>? Images {get; set;} = null;
    public List<BidsForItemDetails>? Bids {get; set;} = null;
    
}
