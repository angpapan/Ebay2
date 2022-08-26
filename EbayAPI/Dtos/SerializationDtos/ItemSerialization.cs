using System.Xml.Serialization;
using EbayAPI.Models;
using Microsoft.IdentityModel.Tokens;

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
    [JsonIgnore, XmlIgnore]
    private string? _BuyPrice;

    [XmlElement("Buy_Price"), JsonIgnore]
    public string BuyPrice
    {
        get
        {
            return (string)_BuyPrice;
        }
        set
        {
            _BuyPrice = value;
        }
    }
    [JsonPropertyName("BuyPrice"), XmlIgnore]
    public string? JsonBuyPrice { get; set; }
    [XmlElement("Number_of_Bids")]
    public int NumberOfBids { get; set; }

    [XmlArray("Bids")]
    [XmlArrayItem("Bid")]
    public List<BidSerialization> Bids { get; set; }
    public SellerLocationSerialization Location { get; set; }
    public string Country { get; set; }
    
    [JsonIgnore, XmlIgnore]
    private string? _Started;
    
    [JsonIgnore]
    public string Started
    {
        get
        {
            return (string)_Started;
        }
        set
        {
            _Started = value;
        }
    }
    
    [JsonPropertyName("Started"), XmlIgnore]
    public string? JsonStarted { get; set; }
    public string Ends { get; set; }
    public SellerSerialization Seller { get; set; }
    public string Description { get; set; }

    public bool StartedSpecified => !_Started.IsNullOrEmpty();
    public bool BuyPriceSpecified => !_BuyPrice.IsNullOrEmpty();

    public ItemSerialization(){}
    public ItemSerialization(Item item)
    {
        ItemId = item.ItemId;
        Name = item.Name;
        Currently = item.Price.ToString("C");
        Category = item.ItemCategories.Select(i => i.Category.Name).ToList();
        FirstBid = item.FirstBid.ToString("C");
        _BuyPrice = item.BuyPrice?.ToString("C");
        JsonBuyPrice = item.BuyPrice?.ToString("C");
        NumberOfBids = item.Bids?.Count ?? 0;
        Bids = item.Bids == null ? new List<BidSerialization>() : item.Bids.Select(b => new BidSerialization(b)).ToList();
        Location = new SellerLocationSerialization(item);
        Country = item.Country;
        _Started = item.Started?.ToString("MMM'-'dd'-'y HH:mm:ss");
        JsonStarted = item.Started?.ToString("MMM'-'dd'-'y HH:mm:ss");
        Ends = item.Ends.ToString("MMM'-'dd'-'y HH:mm:ss");
        Seller = new SellerSerialization(item.Seller);
        Description = item.Description;
    }
}