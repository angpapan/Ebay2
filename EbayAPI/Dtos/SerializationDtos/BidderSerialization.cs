using System.Xml.Serialization;
using EbayAPI.Models;

namespace EbayAPI.Dtos.SerializationDtos;

[XmlRoot("Bidder")]
public class BidderSerialization
{
    [XmlAttribute]
    public int Rating { get; set; }
    
    [XmlAttribute("UserID")]
    public string Username { get; set; }
    
    public string Location { get; set; }
    public string Country { get; set; }
    
    public BidderSerialization() {}
    public BidderSerialization(User user)
    {
        Rating = (int)user.BidderRating;
        Username = user.Username;
        Location = user.City;
        Country = user.Country;
    }
}