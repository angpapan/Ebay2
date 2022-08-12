using EbayAPI.Models;
using EbayAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace EbayAPI.Data;
public class EbayAPIDbContext : DbContext
{
    public EbayAPIDbContext(DbContextOptions<EbayAPIDbContext> options) : base(options) { }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemsCategories> ItemsCategories { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<UserVisitedItems> UserVisitedItems { get; set; }
    public DbSet<Message> Messages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemsCategories>()
            .HasKey(nameof(EbayAPI.Models.ItemsCategories.ItemId), nameof(EbayAPI.Models.ItemsCategories.CategoryId));

        modelBuilder.Entity<UserVisitedItems>()
            .HasKey(nameof(EbayAPI.Models.UserVisitedItems.UserId), nameof(EbayAPI.Models.UserVisitedItems.ItemId));

        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, Name = "Administrator" },
            new Role { RoleId = 2, Name = "User" }
        );

        modelBuilder.Entity<User>().HasData(
            new User {
                UserId = 1,
                Username = "admin", 
                Password = GlobalService.ComputeSha256Hash("admin"), 
                FirstName = "Admin", 
                LastName = "Admin", 
                Email = "admin@EbayAPI.com",
                PhoneNumber = "0123456789", 
                Street = "AdminStreet", 
                StreetNumber = 666, 
                City = "AdminCity", 
                PostalCode = "666",
                Country = "AdminCountry", 
                Enabled = true, 
                RoleId = 1, 
                VATNumber ="123456789"
            }
    );

        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Holidays" },
            new Category { CategoryId = 2, Name = "Clothes" },
            new Category { CategoryId = 3, Name = "Electronic"},
            new Category { CategoryId = 4, Name = "Vehicle"},
            new Category { CategoryId = 5, Name = "House"}
            // TODO ::: get categories from dataset
        );

    modelBuilder.Entity<Item>().HasData(
            new Item {
                ItemId = 132,
                Name = "Hat",
                BuyPrice = 15,
                FirstBid = 12,
                Location = "Athens",
                Country = "Greece",
                Description = "A very nice red hat",
                Ends = new DateTime(2022-03-02),
                Latitude = new decimal(1.3),
                Longitude = new decimal(1.6),
                SellerId = 1
            }
    );
    }
    
}