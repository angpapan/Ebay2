using AutoMapper;
using EbayAPI.Data;
using EbayAPI.Dtos;
using EbayAPI.Helpers;
using EbayAPI.Models;
using EbayAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using EbayAPI.Dtos.ItemDtos;
using EbayAPI.Helpers.Authorize;

namespace EbayAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("item")]
    public class ItemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly EbayAPIDbContext _dbContext;
        private readonly ILogger<ItemService> _logger;
        private readonly ItemService _itemService;
        

        public ItemController(IMapper mapper, EbayAPIDbContext dbContext, ILogger<ItemService> logger,
            ItemService itemService, RecommendationService recommendationService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
            _itemService = itemService;
        }

        [HttpGet("/full/{id}", Name = "GetItemFullDetails")]
        public async Task<ItemDetailsFull> GetFullItemDetails(int id)
        {
            return await _itemService.GetDetailsFullAsync(id , (User)HttpContext.Items["User"]);
        }

        [HttpGet ("{id}", Name = "GetSimpleItem")]
        [AllowAnonymous]
        public async Task<ItemDetails> GetItemDetails(int id)
        {
            return await _itemService.GetDetailsAsync(id, false);
            //return _mapper.Map<ItemDetails>(details);
        }

        [HttpPost("newAuction", Name = "CreateAuction")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAuction([FromBody]ItemAddition newItem)
        {
            User? seller = (User?) HttpContext.Items["User"];
            
            int newId = await _itemService.InsertItem(seller.UserId,newItem);
            
            return Ok($"A new auction has been created successful for item with id : {newId}!");
        }

        [HttpPost("/category/{id:int}", Name = "GetItemsByCategory")]
        [AllowAnonymous]
        public async Task<List<ItemDetailsSimple>> GetItemsByCategory(int id)
        {
            // :: todo -> makeit paged 
            return await _itemService.GetItemsByCategoryId(id);
        }

        
        [HttpPost("/user/{username}", Name = "GetItemsByUsername")]
        [AllowAnonymous]
        public async Task<List<ItemDetailsSimple>>? GetItemsByUserName(string username)
        {
            //:: todo -> make it paged
            return await _itemService.GetItemsByUsername(username);
        }
        
        
        [HttpGet("/price", Name = "GetItemsByPrice")]
        [AllowAnonymous]
        public async Task<List<ItemDetailsSimple>>? GetItemsByPrice([FromQuery] decimal start, decimal end)
        {
            //:: todo -> make it paged
            return await _itemService.GetItemsByPrice(start,end);
        }

        /// <summary>
        /// Get items in paged lists
        /// </summary>
        [HttpGet("search", Name = "SearchItems")]
        [AllowAnonymous]
        public async Task<List<ItemDetailsSimple>>? SearchItemList([FromQuery] ItemListQueryParameters dto)
        {
            PagedList<Item> pageItem = await _itemService.GetSearchItemsList(dto);

            var metadata = new
            {
                pageItem.TotalCount,
                pageItem.PageSize,
                pageItem.CurrentPage,
                pageItem.TotalPages,
                pageItem.HasNext,
                pageItem.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            return _mapper.Map<List<ItemDetailsSimple>>(pageItem);
        }
        
        /// <summary>
        /// Create a new item for sale
        /// </summary>
        [HttpPost, DisableRequestSizeLimit]
        [Helpers.Authorize.Authorize(Roles.User)]
        public async Task<IActionResult> CreateItem([FromForm] ItemAddition dto)
        {
            User? seller = (User?) HttpContext.Items["User"];
            await _itemService.CreateItemAsync(dto, seller);
            return Ok("Success!");
        }   
        
        /// <summary>
        /// Start an already created auction
        /// </summary>
        /// <param name="id">The item id</param>
        [HttpPut("{id}/start")]
        [Helpers.Authorize.Authorize(Roles.User)]
        public async Task<IActionResult> StartAuction(int id)
        {
            User? seller = (User?) HttpContext.Items["User"];
            await _itemService.StartAuction(id, seller!);
            return Ok("Success!");
        }
        
        /// <summary>
        /// Gets a paged list of all the items placed for selling by the
        /// user making the request
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("sells", Name = "SellerItemsList")]
        [Helpers.Authorize.Authorize(Roles.User)]
        public async Task<List<SellerItemListResponse>> SellerItemList([FromQuery] SellerItemListQueryParameters dto)
        {
            User? seller = (User?) HttpContext.Items["User"];
            PagedList<Item> pageItem = await _itemService.GetSellerItemList(dto, seller!);

            var metadata = new
            {
                pageItem.TotalCount,
                pageItem.PageSize,
                pageItem.CurrentPage,
                pageItem.TotalPages,
                pageItem.HasNext,
                pageItem.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            List<SellerItemListResponse> items = _mapper.Map<List<SellerItemListResponse>>(pageItem);
            return items;
        }
        
        /// <summary>
        /// Delete a created item
        /// </summary>
        /// <param name="id">The item id</param>
        [HttpDelete("{id}")]
        [Helpers.Authorize.Authorize(Roles.User)]
        public async Task<IActionResult> DeleteAuction(int id)
        {
            User? seller = (User?) HttpContext.Items["User"];
            await _itemService.DeleteAuction(id, seller!);
            return Ok("Auction deleted successfully!");
        }
        
        /// <summary>
        /// Edit a created auction
        /// </summary>
        /// <param name="id">The item id to edit</param>
        /// <param name="dto">The item details</param>
        [HttpPut("{id}", Name = "UpdateItem")]
        [Helpers.Authorize.Authorize(Roles.User)]
        public async Task<IActionResult> EditAuction(int id, [FromForm] ItemAddition dto)
        {
            User? seller = (User?) HttpContext.Items["User"];
            await _itemService.EditAuction(id, dto, seller!);
            return Ok("Auction edited successfully!");
        }
        
        /// <summary>
        /// Gets the editable info of an item
        /// </summary>
        /// <param name="id">The item id</param>
        /// <returns></returns>
        [HttpGet("edit-info/{id}", Name = "GetItemInfoForEdit")]
        [Helpers.Authorize.Authorize(Roles.User)]
        public async Task<ItemToEditResponseDto> GetEditInfo(int id)
        {
            User? seller = (User?) HttpContext.Items["User"];
            return await _itemService.GetEditInfo(id, seller!);
        }
        
        /// <summary>
        /// Delete an image from an item
        /// </summary>
        /// <param name="item_id">The item id</param>
        /// <param name="image_id">The image id</param>
        /// <returns></returns>
        [HttpDelete("{item_id}/image/{image_id}", Name = "DeleteItemImage")]
        [Helpers.Authorize.Authorize(Roles.User)]
        public async Task<IActionResult> DeleteImageFromItem(int item_id, int image_id)
        {
            User? seller = (User?) HttpContext.Items["User"];
            await _itemService.DeleteImageFromItem(item_id, image_id, seller!);
            return Ok("Image deleted successfully.");
        }
        
        /// <summary>
        /// Get user based recommended items
        /// </summary>
        /// <param name="num">The number of items to recommend</param>
        /// <returns></returns>
        [HttpGet("recommended", Name = "GetRecommendedItems")]
        [Helpers.Authorize.Authorize]
        public async Task<List<ItemBoxDto>> Recommend(int num = 5)
        {
            User? user = (User?) HttpContext.Items["User"];
            return await _itemService.GetRecommendedItems(user!, num);
        }
        
        /// <summary>
        /// Get the most recently submitted items
        /// </summary>
        /// <param name="num">The number of items to recommend</param>
        /// <returns></returns>
        [HttpGet("new", Name = "GetNewItems")]
        public async Task<List<ItemBoxDto>> NewItems(int num = 5)
        {
            return await _itemService.GetNewItems(num);
        }
        
        /// <summary>
        /// Gets the active items with the most bids
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        [HttpGet("hot", Name = "GetHotItems")]
        public async Task<List<ItemBoxDto>> HotItems(int num = 5)
        {
            return await _itemService.GetHotItems(num);
        }
    }
    
}