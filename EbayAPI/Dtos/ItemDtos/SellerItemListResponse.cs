using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class SellerItemListResponse
{
    
    [Required] public int ItemId { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Description { get; set; }
    [Required] public decimal Price { get; set; }
    [Required] public DateTime? Started { get; set; }
    [Required] public DateTime? Ends { get; set; }
    [Required] public bool HasBids { get; set; }
    [Required] public string? Image {get; set;} = null;
    
}
