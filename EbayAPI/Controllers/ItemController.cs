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
using Microsoft.OpenApi.Any;

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
            ItemService itemService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
            _itemService = itemService;
        }

        [HttpGet("full/{id}", Name = "GetItemFullDetails")]
        [Helpers.Authorize.Authorize(Roles.User , Roles.Administrator )]
        public async Task<ItemDetailsFull> GetFullItemDetails(int id)
        {
            return await _itemService.GetDetailsFullAsync(id , (User?)HttpContext.Items["User"]);
        }

        [HttpGet ("{id}", Name = "GetSimpleItem")]
        [AllowAnonymous]
        public async Task<ItemDetails> GetItemDetails(int id)
        {
            return await _itemService.GetDetailsAsync(id);
            //return _mapper.Map<ItemDetails>(details);
        }

        
        
        
        
        [HttpGet("user/{username}", Name = "GetItemsByUsername")]
        [AllowAnonymous]
        public async Task<List<SellerItemListResponse>> GetItemsByUserName(string username, [FromQuery] SellerItemListQueryParameters dto)
        {
            PagedList<Item> userItems = await _itemService.GetItemsByUsername(dto, username); 
            var metadata = new
            {
                userItems.TotalCount,
                userItems.PageSize,
                userItems.CurrentPage,
                userItems.TotalPages,
                userItems.HasNext,
                userItems.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
            Console.WriteLine("Users item has been send");
            return _mapper.Map<List<SellerItemListResponse>>(userItems);
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
        [HttpPost(""), DisableRequestSizeLimit]
        [Helpers.Authorize.Authorize(Roles.User)]
        public async Task<IActionResult> CreateItem([FromForm] ItemAddition dto)
        {
            User? seller = (User?) HttpContext.Items["User"];
            await _itemService.CreateItemAsync(dto, seller);
            return Ok("Success!");
        }   
        
        /// <summary>
        /// Start a cerated auction
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
        
        
        [HttpGet("sells", Name = "SellerItemsList")]
        [Helpers.Authorize.Authorize(Roles.User)]
        public async Task<List<SellerItemListResponse>> SearchItemList([FromQuery] SellerItemListQueryParameters dto)
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
        [HttpPut("{id}")]
        [Helpers.Authorize.Authorize(Roles.User)]
        public async Task<IActionResult> EditAuction(int id, [FromForm] ItemAddition dto)
        {
            User? seller = (User?) HttpContext.Items["User"];
            await _itemService.EditAuction(id, dto, seller!);
            return Ok("Auction edited successfully!");
        }

        [HttpGet("edit-info/{id}")]
        [Helpers.Authorize.Authorize(Roles.User)]
        public async Task<ItemToEditResponseDto> GetEditInfo(int id)
        {
            User? seller = (User?) HttpContext.Items["User"];
            return await _itemService.GetEditInfo(id, seller!);
        }

        [HttpDelete("{item_id}/image/{image_id}")]
        [Helpers.Authorize.Authorize(Roles.User)]
        public async Task<IActionResult> DeleteImageFromItem(int item_id, int image_id)
        {
            User? seller = (User?) HttpContext.Items["User"];
            await _itemService.DeleteImageFromItem(item_id, image_id, seller!);
            return Ok("Image deleted successfully.");
        }


    }
    
}