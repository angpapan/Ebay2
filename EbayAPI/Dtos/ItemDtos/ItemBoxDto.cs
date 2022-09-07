using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class ItemBoxDto
{
    
    [Required] public int ItemId { get; set; }
    [Required] public string Name { get; set; }
    [Required] public decimal Price { get; set; }
    [Required] public DateTime Ends { get; set; }
    [Required] public string? Image {get; set;} = null;
    
}
