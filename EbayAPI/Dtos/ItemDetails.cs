using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class ItemDetails
{
    
    [Required] public int ItemId { get; set; }
    [Required] public string Name { get; set; }
    public decimal? BuyPrice {get; set;} = null;
    [Required] public decimal FirstBid {get; set;}
    public string Location { get; set; }
    public string? Country { get; set; } = null;
    [Required] public string Description { get; set; }
    public DateTime? Started {get; set;} = null;
    [Required] public DateTime Ends {get; set;}
    public decimal? Latitude { get; set; } = null;
    public decimal? Longitude { get; set; } = null;
    [Required] public int SellerId { get; set; }
    public List<ItemsCategories>? ItemCategories {get; set;} = null;
    public List<Image>? Images {get; set;} = null;
    
}
