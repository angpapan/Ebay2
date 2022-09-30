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
    public DbSet<UserBidLatent> UserBidLatents { get; set; }
    public DbSet<ItemBidLatent> ItemBidLatents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemsCategories>()
            .HasKey(nameof(EbayAPI.Models.ItemsCategories.ItemId), nameof(EbayAPI.Models.ItemsCategories.CategoryId));

        modelBuilder.Entity<UserVisitedItems>()
            .HasKey(nameof(EbayAPI.Models.UserVisitedItems.UserId), nameof(EbayAPI.Models.UserVisitedItems.ItemId), nameof(EbayAPI.Models.UserVisitedItems.Dt));

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
    }
    
}
