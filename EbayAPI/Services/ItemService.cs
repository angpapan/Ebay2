#pragma warning disable CS1591
using EbayAPI.Data;
using EbayAPI.Dtos;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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


  
    public async Task<ItemDetailsFull> GetDetailsFullAsync(string id, User? userRequests)
    {
        Item item = _dbContext.Items
            .Where(i => i.ItemId == id)
            .SingleOrDefault();
        if( item == null )
            throw new KeyNotFoundException($"Item {id} not found!");
            
        if( userRequests == null || userRequests.Roles != Roles.Administrator || userRequests.UserId != item.SellerId )
            throw new KeyNotFoundException($"unauthorized access to item {id}!");
            
        return _mapper.Map<ItemDetailsFull>(item);
        
    }
    
    
    public async Task<ItemDetails> GetDetailsAsync(string id, bool? Simple)
    {
        Item item = _dbContext.Items
            .Where(i => i.ItemId == id)
            .SingleOrDefault();
        if( item == null )
            throw new KeyNotFoundException($"Item {id} not found!");
        
        // return simple version of an item 
        if ( Simple != null && Simple == true)
            return _mapper.Map<ItemDetails>(_mapper.Map<ItemDetailsSimple>(item));
        
        // return detailed version of an item    
        return _mapper.Map<ItemDetails>(item);
        
    }

    
   
}
