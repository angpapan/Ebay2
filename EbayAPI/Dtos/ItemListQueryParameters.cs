using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class ItemListQueryParameters : QueryPagingParameters
{
    public string OrderBy { get; set; } = "price";
    public string? Search { get; set; } = null;
    public decimal? MinPrice { get; set; } = null;
    public decimal? MaxPrice { get; set; } = null;
    public List<int>? Categories { get; set; } = null;
    public List<string>? Locations { get; set; } = null;
}
