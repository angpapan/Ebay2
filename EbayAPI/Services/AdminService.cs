using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using EbayAPI.Data;
using EbayAPI.Dtos;
using EbayAPI.Helpers;
using EbayAPI.Models;
using AutoMapper;
using EbayAPI.Dtos.SerializationDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EbayAPI.Services;
public class AdminService
{
    private readonly AppSettings _appSettings;
    private readonly EbayAPIDbContext _dbContext;
    private readonly UserService _userService;
    private readonly IMapper _mapper;

    public AdminService(IOptions<AppSettings> appSettings, EbayAPIDbContext dbContext, IMapper mapper, UserService userService)
    {
        _appSettings = appSettings.Value;
        _dbContext = dbContext;
        _mapper = mapper;
        _userService = userService;
    }

    /// <summary>
    /// enabling a newly registered user 
    /// </summary>
    /// <param name="username"></param>
    /// <exception cref="BadHttpRequestException"></exception>
    public async Task EnableUser(string username)
    {
        User user = _dbContext.Users
            .Where(u => u.Username == username)
            .SingleOrDefault();
        
        if (user == null)
            throw new BadHttpRequestException("Invalid username.");

        user.Enabled = true;
        
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// return all users with details so admin can have access on them 
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public async Task<PagedList<User>> GetAllUsers(UserListQueryParameters parameters)
    {
        IQueryable<User> users = _dbContext.Users;
        
        if (parameters.Search != null)
        {
            users = users.Where(u => u.Username.Contains(parameters.Search) || u.FirstName.Contains(parameters.Search) ||
                             u.LastName.Contains(parameters.Search) || u.Email.Contains(parameters.Search));
        }

        QuerySortHelper<User> usr = new QuerySortHelper<User>();
        users = usr.QuerySort(users, parameters.OrderBy);

        PagedList<User> userPage =
            PagedList<User>.ToPagedList(users, parameters.PageNumber, parameters.PageSize);

        return userPage;
    }


    public async Task<string> ExtractItemInfo(List<int> item_ids, string type)
    {
        List<Item> items = await _dbContext.Items
            .Include(i => i.Seller)
            .Include(i => i.Bids)
            .ThenInclude(b => b.Bidder)
            .Include(i => i.ItemCategories)
            .ThenInclude(ic => ic.Category)
            .Where(i => item_ids.Contains(i.ItemId))
            .ToListAsync();

        ItemListSerialization lista = new ItemListSerialization(items);        
        if (type == "xml")
        {
            string xml = "";

            XmlSerializer serializer = new XmlSerializer(typeof(ItemListSerialization));
            using (StringWriter sw = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings()
                       {
                           Indent = true,
                           OmitXmlDeclaration = true
                       }))
                {
                    serializer.Serialize(writer, lista, new XmlSerializerNamespaces(new []{XmlQualifiedName.Empty}));
                    xml = sw.ToString();
                }
            }
            return xml;
        }
        else
        {
            return  JsonSerializer.Serialize(lista, new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            });
        }
    }

    public async Task ImportXmlData(int start, int end, bool normalizeToNow)
    {
        string filenameBase = "Assets/Dataset/items-";
        Random random = new Random();
        for (int i = start; i <= end; i++)
        {
            string filename = filenameBase + i + ".xml";
            XmlSerializer serializer = new XmlSerializer(typeof(ItemListSerialization));
            ItemListSerialization items;

            using (Stream reader = new FileStream(filename, FileMode.Open))
            {
                items = (ItemListSerialization)serializer.Deserialize(reader);
            }

            foreach (ItemSerialization item in items.Item)
            {
                Item? exists = _dbContext.Items.SingleOrDefault(i => i.XmlId == item.ItemId);
                if (exists != null) continue;
                
                Item it = new Item();
                it.XmlId = item.ItemId; 
                it.Name = item.Name;
                it.Price = decimal.Parse(item.Currently, NumberStyles.AllowCurrencySymbol | NumberStyles.Number);
                it.BuyPrice = item.BuyPrice != null ? decimal.Parse(item.BuyPrice, NumberStyles.AllowCurrencySymbol | NumberStyles.Number) : null;
                it.FirstBid = decimal.Parse(item.FirstBid, NumberStyles.AllowCurrencySymbol | NumberStyles.Number);
                it.Location = item.Location.Location;
                it.Latitude = item.Location.Latitude != null ? decimal.Parse(item.Location.Latitude) : null;
                it.Longitude = item.Location.Longitude != null ? decimal.Parse(item.Location.Longitude) : null;
                it.Country = item.Country;
                it.Description = item.Description;
                it.Started = DateTime.Parse(item.Started);
                DateTime dtEnd = DateTime.Parse(item.Ends);
                if (normalizeToNow)
                    it.Ends = new DateTime(DateTime.Now.Year, dtEnd.Month, dtEnd.Day, dtEnd.Hour, dtEnd.Minute,
                        dtEnd.Second);
                else
                    it.Ends = dtEnd;

                // check seller
                User? seller = _dbContext.Users.SingleOrDefault(u => u.Username == item.Seller.Username);
                if (seller == null)
                {
                    UserRegister reg = new UserRegister();
                    reg.Username = item.Seller.Username;
                    reg.Password = "123456789";
                    reg.VerifyPassword = "123456789";
                    reg.FirstName = item.Seller.Username;
                    reg.LastName = item.Seller.Username;
                    reg.Email = $"{item.Seller.Username}@test.com";
                    reg.PhoneNumber = "1234567890";
                    reg.Street = $"{item.Seller.Username} Street";
                    reg.StreetNumber = 66;
                    reg.City = item.Location.Location;
                    reg.Country = item.Country;
                    reg.PostalCode = "12345";
                    reg.VATNumber = "123456789";

                    await _userService.Register(reg);
                    seller = _dbContext.Users.SingleOrDefault(u => u.Username == item.Seller.Username);
                    seller.Enabled = true;
                }

                seller.SellerRating = item.Seller.Rating;
                seller.SellerRatingsNum = item.Seller.Rating / random.Next(1, 5);
                _dbContext.Users.Update(seller);
                it.SellerId = seller.UserId;
                
                
                // Create item
                _dbContext.Items.Add(it);
                _dbContext.SaveChanges();
                
                // add categories
                List<ItemsCategories> ics = new List<ItemsCategories>();
                foreach (string category in item.Category)
                {
                    Category? cat = _dbContext.Categories.SingleOrDefault(c => c.Name == category);
                    if (cat == null)
                    {
                        Category newCat = new Category();
                        newCat.Name = category;
                        _dbContext.Categories.Add(newCat);
                        _dbContext.SaveChanges();

                        ItemsCategories ic = new ItemsCategories();
                        ic.ItemId = it.ItemId;
                        ic.CategoryId = newCat.CategoryId;
                        
                        if(!ics.Select(i=>i.CategoryId).ToList().Contains(newCat.CategoryId))
                            ics.Add(ic);
                    }
                    else
                    {
                        ItemsCategories ic = new ItemsCategories();
                        ic.ItemId = it.ItemId;
                        ic.CategoryId = cat.CategoryId;
                        if(!ics.Select(i=>i.CategoryId).ToList().Contains(cat.CategoryId))
                            ics.Add(ic);
                    }
                }
                _dbContext.ItemsCategories.AddRange(ics);
                _dbContext.SaveChanges();
                
                // bids
                List<Bid> bs = new List<Bid>();
                foreach (BidSerialization bid in item.Bids)
                {
                    Bid b = new Bid();
                    b.Time = DateTime.Parse(bid.Time);
                    b.Amount = decimal.Parse(bid.Amount, NumberStyles.AllowCurrencySymbol | NumberStyles.Number);
                    b.ItemId = it.ItemId;
                    
                    // check bidder user
                    User? bidder = _dbContext.Users.SingleOrDefault(u => u.Username == bid.Bidder.Username);
                    if (bidder == null)
                    {
                        UserRegister reg = new UserRegister();
                        reg.Username = bid.Bidder.Username;
                        reg.Password = "123456789";
                        reg.VerifyPassword = "123456789";
                        reg.FirstName = bid.Bidder.Username;
                        reg.LastName = bid.Bidder.Username;
                        reg.Email = $"{bid.Bidder.Username}@test.com";
                        reg.PhoneNumber = "1234567890";
                        reg.Street = $"{bid.Bidder.Username} Street";
                        reg.StreetNumber = 66;
                        reg.City = bid.Bidder.Location ?? "Unknown";
                        reg.Country = bid.Bidder.Country ?? "Unknown";
                        reg.PostalCode = "12345";
                        reg.VATNumber = "123456789";

                        await _userService.Register(reg);
                        bidder = _dbContext.Users.SingleOrDefault(u => u.Username == bid.Bidder.Username);
                        bidder.Enabled = true;
                    }
                    
                    bidder.BidderRating = bid.Bidder.Rating;
                    bidder.BidderRatingsNum = bid.Bidder.Rating / random.Next(1, 5);
                    _dbContext.Users.Update(bidder);

                    b.UserId = bidder!.UserId;
                    bs.Add(b);
                }
                _dbContext.Bids.AddRange(bs);
                _dbContext.SaveChanges();
            }
        }
    }


}