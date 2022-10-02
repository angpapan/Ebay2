using Microsoft.EntityFrameworkCore;

namespace EbayAPI.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        public int CategoryId { get; set; }

        [StringLength(150), Required]
        public string Name { get; set; }

        [JsonIgnore] public virtual List<ItemsCategories> CategoryItems { get; set; } = new List<ItemsCategories>();
    }
}