using System.Xml.Serialization;
using EbayAPI.Models;

namespace EbayAPI.Dtos.SerializationDtos;

[XmlRoot("Seller")]
public class SellerSerialization
{
    [XmlAttribute]
    public int Rating { get; set; }
    
    [XmlAttribute("UserID")]
    public string Username { get; set; }
    
    public SellerSerialization(){}
    public SellerSerialization(User user)
    {
        Rating = (int)user.BidderRating;
        Username = user.Username;
    }
}