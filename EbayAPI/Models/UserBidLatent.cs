namespace EbayAPI.Models
{
    public class UserBidLatent
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string LatentFeatures {get; set;}
    }
}