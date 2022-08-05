using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class ItemDetails
{
    
    [Required] public int ItemId { get; set; }
    [Required] public string Name { get; set; }
    public decimal? BuyPrice {get; set;} = null;
    public List<ItemsCategories>? ItemCategories {get; set;} = null;
    public List<Image>? Images {get; set;} = null;
    
}
