using EbayAPI.Models;

namespace EbayAPI.Dtos.ItemDtos;

public class SellerItemListQueryParameters : QueryPagingParameters
{
    public string OrderBy { get; set; } = "ItemId desc";
    public string? Search { get; set; } = null;
}
