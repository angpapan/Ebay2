namespace EbayAPI.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [StringLength(150), Required]
        public string Name { get; set; }

        public int? GenericId { get; set; } = null;
        
        [JsonIgnore, ForeignKey("GenericId")]
        public virtual Category? Generic { get; set; }

        [JsonIgnore] 
        public virtual List<ItemsCategories>? CategoryItems { get; set; }
    }
}