using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class ItemDetailsSimple
{
    
    [Required] public int ItemId { get; set; }
    [Required] public string Name { get; set; }
    public decimal? BuyPrice {get; set;} = null;
    public Image Images {get; set;} = null;
    
}
