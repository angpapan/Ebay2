using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class UserListQueryParameters : QueryPagingParameters
{
    public string OrderBy { get; set; } = "DateCreated desc";
    public string? Search {get; set;}
}
