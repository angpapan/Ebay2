using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;


namespace EbayAPI.Models
{
    [
        Index(nameof(Name)),
        Index(nameof(Price)),
        Index(nameof(Country)),
    ]
    public class Item
    {
        public int ItemId { get; set; }

        [StringLength(600), Required]
        public string Name { get; set; }
        
        [Column(TypeName = "DECIMAL(19, 2)")]
        public decimal? BuyPrice {get; set;} = null;

        [Required, Column(TypeName = "DECIMAL(19, 2)")]
        public decimal FirstBid {get; set;}
        
        [Required, Column(TypeName = "DECIMAL(19, 2)")]
        public decimal Price { get; set; }

        [StringLength(500), Required]
        public string Location { get; set; }

        [StringLength(200), Required]
        public string Country { get; set; }

        [MaxLength, Required]
        public string Description { get; set; }

        public DateTime? Started {get; set;} = null;
        [Required]
        public DateTime Ends {get; set;}

        [Column(TypeName = "DECIMAL(8, 6)")] 
        public decimal? Latitude { get; set; } = null;

        [Column(TypeName = "DECIMAL(9, 6)")] 
        public decimal? Longitude { get; set; } = null;
        
        [Required]
        public int SellerId { get; set; }
        [ForeignKey("SellerId"), JsonIgnore]
        public virtual User Seller {get; set;}
        
        [JsonIgnore]
        public virtual List<ItemsCategories> ItemCategories {get; set;}

        [JsonIgnore] public virtual List<Bid> Bids { get; set; } = new List<Bid>();
        [JsonIgnore]
        public virtual List<Image>? Images {get; set;}
        [JsonIgnore]
        public virtual List<UserVisitedItems>? VisitedByUsers {get; set;}

        /// <summary>
        /// This field only added to keep the item id from the xml import
        /// to avoid dropping and recreating the items.
        /// </summary>
        [JsonIgnore] public int? XmlId { get; set; } = null;
    }
}
