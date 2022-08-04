using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class UserDetails
{
    [Required] public string Username { get; set; }
    public string? FirstName { get; set; } = null;
    public string? LastName {get; set;} = null;
    [Required] public string Email {get; set;}
    public string? PhoneNumber {get; set;} = null;
    public string? Street {get; set;} = null;
    public int? StreetNumber {get; set;} = null;
    [Required] public string City {get; set;}
    public string? PostalCode {get; set;} = null;
    [Required] public string Country {get; set;}
    public string? VATNumber { get; set; } = null;
    public bool? Enabled { get; set; } = null;
    [Required] public int SellerRatingsNum {get; set; }
    [Required] public double SellerRating {get; set; }
    [Required] public int BidderRatingsNum {get; set; }
    [Required] public double BidderRating {get; set; }
    [Required] public DateTime DateCreated {get; set; }
}