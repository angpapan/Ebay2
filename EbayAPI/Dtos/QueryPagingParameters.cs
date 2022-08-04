namespace EbayAPI.Dtos;

public abstract class QueryPagingParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}