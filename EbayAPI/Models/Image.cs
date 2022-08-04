namespace EbayAPI.Models
{
    public class Image
    {
        public int ImageId { get; set; }

        [Required]
        public byte[] ImageBytes { get; set; }
        
        [StringLength(200), Required]
        public string ImageType { get; set; }

        [Required]
        public int ItemId {get; set;}
        [JsonIgnore, ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
    }
}