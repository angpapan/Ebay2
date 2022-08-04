namespace EbayAPI.Dtos;

public class UserDetailsUser
{
    public string Username { get; set; }
    public string Email {get; set;}
    public string City {get; set;}
    public string Country {get; set;}
    public int SellerRatingsNum {get; set; }
    public double SellerRating {get; set; }
    public int BidderRatingsNum {get; set; }
    public double BidderRating {get; set; }
    public DateTime DateCreated {get; set; }
}