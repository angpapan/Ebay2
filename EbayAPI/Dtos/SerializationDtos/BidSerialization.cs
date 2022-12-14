using System.Xml.Serialization;
using EbayAPI.Models;

namespace EbayAPI.Dtos.SerializationDtos;

[XmlRoot("Bid")]
public class BidSerialization
{
    public BidderSerialization Bidder { get; set; }
    public string Time { get; set; }
    public string Amount { get; set; }
    
    public BidSerialization(){}
    public BidSerialization(Bid bid)
    {
        Bidder = new BidderSerialization(bid.Bidder);
        Time = bid.Time.ToString("MMM'-'dd'-'y HH:mm:ss");
        Amount = bid.Amount.ToString("C");
    }
}