namespace EbayAPI.Models
{
    public class ItemBidLatent
    {
        [Key]
        public int ItemId { get; set; }
        [Required]
        public string LatentFeatures {get; set;}
    }
}