namespace EbayAPI.Models
{
    public class ItemViewLatent
    {
        [Key]
        public int ItemId { get; set; }
        [Required]
        public string LatentFeatures {get; set;}
    }
}