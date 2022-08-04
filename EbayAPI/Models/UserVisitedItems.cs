namespace EbayAPI.Models
{
    public class UserVisitedItems
    {
        // Composite key Category, Item
        [Required, Column(Order=1)]
        public int UserId { get; set; }
        [Required, Column(Order=2)]
        public int ItemId { get; set; }
        
        [ForeignKey("UserId"), JsonIgnore]
        public virtual User User { get; set; }
        [ForeignKey("ItemId"), JsonIgnore]
        public virtual Item Item { get; set; }
        
        [Required]
        public DateTime Dt { get; set; }
    }
}