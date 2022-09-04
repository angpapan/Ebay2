namespace EbayAPI.Models
{
    public class UserViewLatent
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string LatentFeatures {get; set;}
    }
}