namespace EbayAPI.Dtos.BidsDtos;

public class BidsForItemDetails
{
    public BidderDto Bidder { get; set; }
    public decimal Amount {get; set;}
    public DateTime Time {get; set;}
}