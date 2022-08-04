namespace EbayAPI.Models
{
    public class Bid
    {
        public int BidId { get; set; }

        [Required, Column(TypeName = "DECIMAL(19, 2)")]
        public decimal Amount {get; set;}

        [Required]
        public DateTime Time {get; set;}

        [Required]
        public int UserId {get; set;}
        [ForeignKey("UserId"), JsonIgnore]
        public virtual User Bidder {get; set;}
        
        [Required]
        public int ItemId {get; set;}
        [ForeignKey("ItemId"), JsonIgnore]
        public virtual Item Item {get; set;}
    }
}