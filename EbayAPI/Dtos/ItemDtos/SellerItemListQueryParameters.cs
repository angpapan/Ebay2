using EbayAPI.Models;

namespace EbayAPI.Dtos.ItemDtos;

public class SellerItemListQueryParameters : QueryPagingParameters
{
    public string OrderBy { get; set; } = "ItemId desc";
    public decimal? MinPrice { get; set; } = null;
    public decimal? MaxPrice { get; set; } = null;
    public string? Search { get; set; } = null;
}
