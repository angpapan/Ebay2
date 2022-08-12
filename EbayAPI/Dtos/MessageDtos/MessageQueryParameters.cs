namespace EbayAPI.Dtos.MessageDtos;

public class MessageQueryParameters : QueryPagingParameters
{
    /// <summary>
    /// The search string to search for sender/receiver or subject
    /// </summary>
    public string? search { get; set; } = null;
}