using System.Xml.Serialization;
using EbayAPI.Models;

namespace EbayAPI.Dtos.SerializationDtos;

[XmlRoot("Bids")]
public class BidListSerialization
{
    [XmlElement, JsonPropertyName("Bids")]
    public List<BidSerialization> Bid { get; set; }

    public BidListSerialization()
    {
        // Bid = new List<BidSerialization>();
    }

    public BidListSerialization(List<Bid> bids)
    {
        List<BidSerialization> _bids = new List<BidSerialization>();
        foreach (Bid bid in bids)
        {
            BidSerialization b = new BidSerialization(bid);
            _bids.Add(b);
        }

        Bid = _bids;
    }
}