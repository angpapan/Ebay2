using System.Xml.Serialization;
using EbayAPI.Models;

namespace EbayAPI.Dtos.SerializationDtos;

[XmlRoot("Items")]
public class ItemListSerialization
{
    [XmlElement, JsonPropertyName("Items")]
    public List<ItemSerialization> Item { get; set; }
    
    public  ItemListSerialization() {}

    public ItemListSerialization(List<Item> items)
    {
        List<ItemSerialization> _items = new List<ItemSerialization>();
        foreach (Item item in items)
        {
            ItemSerialization it = new ItemSerialization(item);
            _items.Add(it);
        }

        Item = _items;
    }
}