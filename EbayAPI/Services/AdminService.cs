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
    private readonly IMapper _mapper;

    public AdminService(IOptions<AppSettings> appSettings, EbayAPIDbContext dbContext, IMapper mapper)
    {
        _appSettings = appSettings.Value;
        _dbContext = dbContext;
        _mapper = mapper;
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


}