using EbayAPI.Models;

namespace EbayAPI.Dtos.BidDtos;

public class BidderItemListQueryParameters : QueryPagingParameters
{
    public string OrderBy { get; set; } = "Ends desc";
    public string? Search { get; set; } = null;
}
