namespace EbayAPI.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        [StringLength(100), Required]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual List<User>? Users { get; set; }
        
    }
}