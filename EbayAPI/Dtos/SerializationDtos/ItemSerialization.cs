using System.Xml.Serialization;
using EbayAPI.Models;

namespace EbayAPI.Dtos.SerializationDtos;

[XmlRoot("Item")]
public class ItemSerialization
{
    [XmlAttribute]
    public int ItemId { get; set; }
    public string Name { get; set; }
    public string Currently { get; set; }
    [XmlElement]
    public List<string> Category { get; set; }
    [XmlElement("First_Bid")]
    public string FirstBid { get; set; }
    [XmlElement("Number_Of_Bids")]
    public int NumberOfBids { get; set; }

    public List<BidSerialization> Bids { get; set; }
    public SellerLocationSerialization Location { get; set; }
    public string Country { get; set; }
    
    [JsonIgnore, XmlIgnore]
    private DateTime? _Started;
    
    [JsonIgnore]
    public DateTime Started
    {
        get
        {
            return (DateTime)_Started;
        }
        set
        {
            _Started = value;
        }
    }
    
    [JsonPropertyName("Started"), XmlIgnore]
    public DateTime? JsonStarted { get; set; }
    public DateTime Ends { get; set; }
    public SellerSerialization Seller { get; set; }
    public string Description { get; set; }

    public bool StartedSpecified
    {
        get
        {
            return _Started.HasValue;
        }
    }
    
    public ItemSerialization(){}
    public ItemSerialization(Item item)
    {
        ItemId = item.ItemId;
        Name = item.Name;
        Currently = item.Price.ToString("C");
        Category = item.ItemCategories.Select(i => i.Category.Name).ToList();
        FirstBid = item.FirstBid.ToString("C");
        NumberOfBids = item.Bids == null ? 0 : item.Bids.Count;
        Bids = item.Bids == null ? new List<BidSerialization>() : item.Bids.Select(b => new BidSerialization(b)).ToList();
        Location = new SellerLocationSerialization(item);
        Country = item.Country;
        _Started = item.Started;
        JsonStarted = item.Started;
        Ends = item.Ends;
        Seller = new SellerSerialization(item.Seller);
        Description = item.Description;
    }
}