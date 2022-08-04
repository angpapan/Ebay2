using EbayAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace EbayAPI.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        public int UserId { get; set; }

        [StringLength(50), Required]
        public string Username { get; set; }

        [StringLength(64), Required] 
        public string Password { get; set; }

        [StringLength(50), Required]
        public string FirstName {get; set;}

        [StringLength(100), Required]
        public string LastName {get; set;}

        [DataType(DataType.EmailAddress), Required]
        public string Email {get; set;}

        [StringLength(20), Required]
        public string PhoneNumber {get; set;}

        [StringLength(200), Required]
        public string Street {get; set;}

        [Required]
        public int StreetNumber {get; set;}

        [StringLength(200), Required]
        public string City {get; set;}

        [DataType(DataType.PostalCode), Required]
        public string PostalCode {get; set;}
        
        [StringLength(200), Required]
        public string Country {get; set;}
        
        [StringLength(14), Required]
        public string VATNumber { get; set; }




        [Required]
        public bool Enabled {get; set;} = false;
        
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;





        [Required]
        public int SellerRatingsNum {get; set; } = 0;
        [Required]
        public double SellerRating {get; set; } = 0;

        [Required]
        public int BidderRatingsNum {get; set; } = 0;
        [Required]
        public double BidderRating {get; set; } = 0;

        [Required] public int RoleId { get; set; } = 2;
        [ForeignKey("RoleId"), JsonIgnore] 
        public virtual Role Role {get; set;} 

        [JsonIgnore]
        public virtual List<Item>? Items {get; set;}
        [JsonIgnore]
        public virtual List<Bid>? Bids {get; set;}
        [JsonIgnore]
        public virtual List<UserVisitedItems>? VisitedItems {get; set;}
    }



}