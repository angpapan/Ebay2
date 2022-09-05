#pragma warning disable CS1591
using System.Linq.Dynamic.Core;
using EbayAPI.Data;
using EbayAPI.Dtos;
using Microsoft.Extensions.Options;
using AutoMapper;
using EbayAPI.Dtos.BidsDtos;
using EbayAPI.Dtos.ImageDtos;
using EbayAPI.Dtos.ItemDtos;
using EbayAPI.Helpers;
using EbayAPI.Helpers.Authorize;
using EbayAPI.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

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
            .Include(item => item.Seller )
            .Include(item=>item.ItemCategories)
            .ThenInclude(categories => categories.Category)
            .Include(item=>item.Images)
            .Include(item=>item.Bids)
            .ThenInclude(bid => bid.Bidder)
            .ThenInclude(user => user.Bids )
            .Where(item =>  item.ItemId == id)
            .SingleOrDefault();
        
        if( item == null )
            throw new KeyNotFoundException($"Item {id} not found!");
            
        if( userRequests == null || (userRequests.RoleId != Roles.Administrator && userRequests.UserId != item.SellerId) )
            throw new KeyNotFoundException($"unauthorized access to item {id} for role {userRequests.RoleId} and userid {userRequests.UserId}!");
        var toR = _mapper.Map<ItemDetailsFull>(item);
        //toR.Bids = toR.Bids.OrderByDescending(i => i.Amount).ToList();
        return toR;

    }


    public async Task<ItemDetails> GetDetailsAsync(int id)
    {
        Item? item = _dbContext.Items
            .Include(i => i.Images)
            .Include(s => s.Seller)
            .Include(c => c.ItemCategories)
            .ThenInclude(i => i.Category)
            .Where(i => i.ItemId == id && (
                                            i.Started != null && 
                                            i.Ends > DateTime.Now && 
                                            i.BuyPrice != null ? i.Price < i.BuyPrice : true)
            )
            .SingleOrDefault()
            ;
       
        
        if( item == null )
            throw new KeyNotFoundException($"Item {id} not found!");
        
        ItemDetails toR = _mapper.Map<ItemDetails>(item);
        
        var l = new List<string>();
        foreach (var im in item.Images)
        {
            l.Add(Convert.ToBase64String(im.ImageBytes));
        }

        toR.Images = l;
        
        return toR;
        }

    

    public async Task<PagedList<Item>>? GetItemsByUsername(SellerItemListQueryParameters dto ,string username)
    {
        
        IQueryable<Item> items = _dbContext.Items
                .Include(i=>i.Seller)
                .Include(i=>i.Images)
                .Where(i =>
                    
                        i.Seller.Username == username && 
                        ( i.BuyPrice == null || i.Price < i.BuyPrice) &&
                        i.Ends > DateTime.Now &&
                        i.Started != null

                )
            ;
        return PagedList<Item>.ToPagedList(items, dto.PageNumber, dto.PageSize);
        
    }
    

    public async Task<PagedList<Item>> GetSearchItemsList(ItemListQueryParameters dto)
    {
        IQueryable<Item> items = _dbContext.Items
            .Include(i => i.ItemCategories)
            .Include(i => i.Images)/*
            .Where(item =>
                item.Ends > DateTime.Now &&
                item.Started != null &&
                (item.BuyPrice == null || item.Price < item.BuyPrice)
                )
            */
            ;

        if(dto.MinPrice != null)
        {
            items = items.Where(i => i.Price >= dto.MinPrice);
        }

        if (dto.MaxPrice != null)
        {
            items = items.Where(i => i.Price <= dto.MaxPrice);
        }

        if(dto.Locations != null)
        {
            items = items.Where(i => dto.Locations.Contains(i.Country));
        }

        if(dto.Categories != null)
        {
            items = items
                .Where(i => i.ItemCategories
                    .Select(ic => ic.CategoryId)
                    .Any(c => dto.Categories.Contains(c)));
        }

        QuerySortHelper<Item> it = new QuerySortHelper<Item>();
        items = it.QuerySort(items, dto.OrderBy);

        PagedList<Item> itemPage =
            PagedList<Item>.ToPagedList(items, dto.PageNumber, dto.PageSize);

        return itemPage;
    }

    public async Task CreateItemAsync(ItemAddition dto, User? seller)
    {
        if (seller == null)
        {
            throw new UnauthorizedAccessException("Please login to sell an item.");
        }

        if (dto.CategoriesId == null || dto.CategoriesId.Count == 0)
        {
            throw new BadHttpRequestException("There should be at least one category.");
        }

        var transaction = _dbContext.Database.BeginTransaction();
        Item item = _mapper.Map<Item>(dto);

        item.SellerId = seller.UserId;
        item.Started = null;
        _dbContext.Items.Add(item);
        await _dbContext.SaveChangesAsync();

        foreach (int category in dto.CategoriesId)
        {
            ItemsCategories ic = new ItemsCategories();
            ic.CategoryId = category;
            ic.ItemId = item.ItemId;
            _dbContext.ItemsCategories.Add(ic);
        }

        if (dto.ImageFiles != null)
        {
            foreach (IFormFile image in dto.ImageFiles)
            {
                Image im = new Image();
                im.ImageType = image.ContentType;
                im.ItemId = item.ItemId;
                using (MemoryStream ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    im.ImageBytes = ms.ToArray();
                }

                _dbContext.Images.Add(im);
            }
            
        }

        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public async Task StartAuction(int id, User seller)
    {
        Item? item = await _dbContext.Items.FindAsync(id);

        if (item == null)
        {
            throw new KeyNotFoundException("No such item found.");
        }
        
        if (item.SellerId != seller.UserId)
        {
            throw new UnauthorizedAccessException("Only the seller can start the auction.");
        }
        
        if (item.Started != null)
        {
            throw new NotSupportedException("The action have already started.");
        }
        
        item.Started = DateTime.Now;
        await _dbContext.SaveChangesAsync();
    }
    
    
    public async Task<PagedList<Item>> GetSellerItemList(SellerItemListQueryParameters dto, User seller)
    {
        IQueryable<Item> items = _dbContext.Items
            .Include((i => i.Bids))
            .Include(i => i.Images)
            .Where(i => i.SellerId == seller.UserId);

        if(dto.Search != null)
        {
            items = items.Where(i => i.Description.Contains(dto.Search) || i.Name.Contains(dto.Search));
        }


        QuerySortHelper<Item> it = new QuerySortHelper<Item>();
        items = it.QuerySort(items, dto.OrderBy);

        PagedList<Item> itemPage =
            PagedList<Item>.ToPagedList(items, dto.PageNumber, dto.PageSize);

        return itemPage;
    }
    
    
    public async Task DeleteAuction(int id, User seller)
    {
        Item? item = await _dbContext.Items
            .Include(i => i.Bids)
            .SingleOrDefaultAsync(i => i.ItemId == id);

        if (item == null)
        {
            throw new KeyNotFoundException("No such item found.");
        }
        
        if (item.SellerId != seller.UserId)
        {
            throw new UnauthorizedAccessException("Only the seller can delete the auction.");
        }
        
        if (item.Bids != null && item.Bids.Count > 0)
        {
            throw new NotSupportedException("The action already has some bids, so it cannot be deleted.");
        }

        _dbContext.Items.Remove(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task EditAuction(int item_id, ItemAddition dto, User seller)
    {
        Item? item = await _dbContext.Items
            .Include(i => i.ItemCategories)
            .Include(i => i.Bids)
            .SingleOrDefaultAsync(i => i.ItemId == item_id);

        if (item == null)
        {
            throw new KeyNotFoundException("No such item found.");
        }
        
        if (item.SellerId != seller.UserId)
        {
            throw new UnauthorizedAccessException("Only the seller can delete the auction.");
        }
        
        if (item.Bids != null && item.Bids.Count > 0)
        {
            throw new NotSupportedException("The action already has some bids, so it cannot be deleted.");
        }
        
        if (dto.CategoriesId == null || dto.CategoriesId.Count == 0)
        {
            throw new BadHttpRequestException("There should be at least one category.");
        }

        // Update item values
        // DO NOT USE mapper because it destroyed old values in unmapped properties
        item.Name = dto.Name;
        item.BuyPrice = dto.BuyPrice;
        item.FirstBid = dto.FirstBid;
        item.Price = dto.FirstBid;
        item.Location = dto.Location;
        item.Country = dto.Country;
        item.Description = dto.Description;
        item.Ends = dto.Ends;
        item.Latitude = dto.Latitude;
        item.Longitude = dto.Longitude;
        
    
        
        // remove old categories
        _dbContext.ItemsCategories.RemoveRange(_dbContext.ItemsCategories
            .Where(ic => ic.ItemId == item.ItemId)
            .ToList()
        );
        // item.ItemCategories = new List<ItemsCategories>();

        List<ItemsCategories> ics = new List<ItemsCategories>();
        foreach (int category in dto.CategoriesId)
        {
            ItemsCategories ic = new ItemsCategories();
            ic.CategoryId = category;
            ic.ItemId = item.ItemId;
            ics.Add(ic);   
            
            // _dbContext.ItemsCategories.Add(ic);
        }

        item.ItemCategories = ics;
        // add new images
        // old images are maintained and must be removed with a different request
        if (dto.ImageFiles != null)
        {
            List<Image> images = item.Images ?? new List<Image>();
            foreach (IFormFile image in dto.ImageFiles)
            {
                Image im = new Image();
                im.ImageType = image.ContentType;
                im.ItemId = item.ItemId;
                using (MemoryStream ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    im.ImageBytes = ms.ToArray();
                }

                images.Add(im);
            }

            item.Images = images;
        }

        _dbContext.Items.Update(item);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ItemToEditResponseDto> GetEditInfo(int item_id, User seller)
    {
        Item? item = await _dbContext.Items
            .Include(i => i.ItemCategories)
            .ThenInclude(ic => ic.Category)
            .Include(i => i.Bids)
            .Include(i => i.Images)
            .SingleOrDefaultAsync(i => i.ItemId == item_id);

        if (item == null)
        {
            throw new KeyNotFoundException("No such item found.");
        }
        
        if (item.SellerId != seller.UserId)
        {
            throw new UnauthorizedAccessException("Only the seller can delete the auction.");
        }
        
        if (item.Bids != null && item.Bids.Count > 0)
        {
            throw new NotSupportedException("The action already has some bids, so it cannot be deleted.");
        }

        ItemToEditResponseDto dto = _mapper.Map<ItemToEditResponseDto>(item);

        List<Category> cats = await _dbContext.Categories
            .Where(c => !item.ItemCategories.Select(ic => ic.CategoryId).ToList().Contains(c.CategoryId))
            .ToListAsync();

        dto.RestCategories = _mapper.Map<List<CategoryDto>>(cats);

        return dto;
    }

    public async Task DeleteImageFromItem(int item_id, int image_id, User seller)
    {
        Item? item = await _dbContext.Items
            .Include(i => i.Images)
            .Include(i => i.Bids)
            .SingleOrDefaultAsync(i => i.ItemId == item_id);

        if (item == null)
        {
            throw new KeyNotFoundException("No such item found.");
        }
        
        if (item.SellerId != seller.UserId)
        {
            throw new UnauthorizedAccessException("Only the seller can delete images for this item.");
        }
        
        if (item.Bids != null && item.Bids.Count > 0)
        {
            throw new NotSupportedException("The auction already has some bids, so it cannot be modified.");
        }

        if (item.Images == null || !item.Images.Select(i => i.ImageId).ToList().Contains(image_id))
        {
            throw new KeyNotFoundException("No such image found.");
        }

        Image? image = await _dbContext.Images.FindAsync(image_id);

        if (image == null)
        {
            throw new KeyNotFoundException("No such image found.");
        }

        _dbContext.Images.Remove(image);
        await _dbContext.SaveChangesAsync();
    }

}
