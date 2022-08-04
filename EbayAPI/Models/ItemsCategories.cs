namespace EbayAPI.Models
{
    public class ItemsCategories
    {
        // Composite key Category, Item
        [Required, Column(Order=1)]
        public int ItemId { get; set; }
        [Required, Column(Order=2)]
        public int CategoryId { get; set; }
        
        [ForeignKey("CategoryId"), JsonIgnore]
        public virtual Category Category { get; set; }
        [ForeignKey("ItemId"), JsonIgnore]
        public virtual Item Item { get; set; }
    }
}