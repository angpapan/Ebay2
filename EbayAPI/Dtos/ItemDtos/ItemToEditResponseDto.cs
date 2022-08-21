using EbayAPI.Dtos.ImageDtos;
using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class ItemToEditResponseDto
{
    
    [Required] public string Name { get; set; }
    public decimal? BuyPrice {get; set;} = null;
    [Required] public decimal FirstBid {get; set;}
    public string Location { get; set; }
    public string? Country { get; set; } = null;
    [Required] public string Description { get; set; }
    [Required] public DateTime Ends {get; set;}
    public decimal? Latitude { get; set; } = null;
    public decimal? Longitude { get; set; } = null;
    [Required] public List<CategoryDto> AddedCategories {get; set;}
    public List<CategoryDto>? RestCategories {get; set;}
    public List<Base64WithIdImageDto>? CurrentImages {get; set;} = null;
}