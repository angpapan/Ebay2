#pragma warning disable CS1591
using EbayAPI.Data;
using EbayAPI.Dtos;
using Microsoft.Extensions.Options;
using AutoMapper;
using EbayAPI.Helpers;
using EbayAPI.Helpers.Authorize;
using EbayAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EbayAPI.Services;
public class ItemService
{
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    private readonly AppSettings _appSettings;
    private readonly EbayAPIDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public ItemService(IOptions<AppSettings> appSettings, EbayAPIDbContext dbContext, IMapper mapper)
    {
        _appSettings = appSettings.Value;
        _dbContext = dbContext;
        _mapper = mapper;
    }


  
    public async Task<ItemDetailsFull> GetDetailsFullAsync(int id, User? userRequests)
    {
        Item item = _dbContext.Items
            .Include(i=>i.ItemCategories)
            .ThenInclude(i=>i.Category)
            .Where(i => i.ItemId == id)
            .SingleOrDefault();
        if( item == null )
            throw new KeyNotFoundException($"Item {id} not found!");
            
        if( userRequests == null || (userRequests.RoleId != Roles.Administrator && userRequests.UserId != item.SellerId) )
            throw new KeyNotFoundException($"unauthorized access to item {id} for role {userRequests.RoleId} and userid {userRequests.UserId}!");
            
        return _mapper.Map<ItemDetailsFull>(item);
        
    }
    
    
    public async Task<ItemDetails> GetDetailsAsync(int id, bool? simple)
    {
        Item item = _dbContext.Items
            .Include(c => c.ItemCategories)
            .ThenInclude(i=>i.Category)
            .Where(i => i.ItemId == id)
            .SingleOrDefault();
       
        
        if( item == null )
            throw new KeyNotFoundException($"Item {id} not found!");
        
        // return simple version of an item 
        if ( simple != null && simple == true)
            return _mapper.Map<ItemDetails>(_mapper.Map<ItemDetailsSimple>(item));
        
        // return detailed version of an item
        ItemDetails toReturn = _mapper.Map<ItemDetails>(item);
        toReturn.Categories = _mapper.Map<List<CategoryBasics>>(item.ItemCategories.Select(i=>i.Category));
        return toReturn;
    }

    public async Task<int> InsertItem(int userId, ItemAddition itemInput)
    {
        
        Item item = _mapper.Map<Item>(itemInput);
        item.ItemId = _dbContext.Items.Max(i => i.ItemId) + 1;
        item.SellerId = userId;

        if (itemInput.CategoriesId != null)
        {
            foreach (var categoryId in itemInput.CategoriesId)
            {
                BindItem(item.ItemId, categoryId);
            }
        }
            
        
        _dbContext.Items.Add(item);
        await _dbContext.SaveChangesAsync();
        return item.ItemId;
    }

    public async Task<List<ItemDetailsSimple>>? GetItemsByCategoryId(int categoryId)
    {
        List<int>? itemIds = _dbContext.ItemsCategories
            .Where(j=>j.CategoryId == categoryId)
            .Select(i=>i.ItemId).ToList();

        List<Item> items = _dbContext.Items
            .Where(i => itemIds.Contains(i.ItemId)).ToList();
        return _mapper.Map<List<ItemDetailsSimple>>(items);
    }

    public async Task<List<ItemDetailsSimple>>? GetItemsByUsername(string username)
    {
        User user = _dbContext.Users.Where(n => n.Username == username).FirstOrDefault();
        if (user == null)
            return null;
        List<Item> items = _dbContext.Items.Where(s=>s.SellerId == user.UserId).ToList();
        return _mapper.Map<List<ItemDetailsSimple>>(items);
        
    }



    private async Task BindItem(int itemid,int catid)
    {
        ItemsCategories newEntry = new ItemsCategories();
        newEntry.CategoryId = catid;
        newEntry.ItemId = itemid;
        _dbContext.ItemsCategories.Add(newEntry);
        await _dbContext.SaveChangesAsync();
    }



}
