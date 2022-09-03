
namespace EbayAPI.Dtos;

public class ItemAddition
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
    public List<int>? CategoriesId {get; set;} = null;
    public List<IFormFile>? ImageFiles {get; set;} = new List<IFormFile>();
}