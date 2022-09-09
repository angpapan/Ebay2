namespace EbayAPI.Dtos.BidDtos;

public class UserBidInfoDto
{
    public int ItemId { get; set; }
    public decimal MaxBid { get; set; }
    public decimal UserMaxBid { get; set; }
    public decimal? BuyPrice { get; set; }
    public DateTime Ends { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string SellerUsername { get; set; }
    [Required] public string? Image {get; set;} = null;
}