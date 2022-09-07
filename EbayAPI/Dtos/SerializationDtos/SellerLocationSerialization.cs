using System.Xml.Serialization;
using EbayAPI.Models;

namespace EbayAPI.Dtos.SerializationDtos;

[XmlRoot("Location")]
public class SellerLocationSerialization
{
    [XmlAttribute]
    public string? Latitude { get; set; }
    
    [XmlAttribute]
    public string? Longitude { get; set; }
    [XmlText]
    public string Location { get; set; }
    
    
    
    public SellerLocationSerialization(){}
    public SellerLocationSerialization(Item item)
    {
        Latitude = item.Latitude != null ? item.Latitude.ToString() : null;
        Longitude = item.Longitude != null ? item.Longitude.ToString() : null;
        Location = item.Location;
    }
}