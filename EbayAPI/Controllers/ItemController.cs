using AutoMapper;
using EbayAPI.Data;
using EbayAPI.Dtos;
using EbayAPI.Models;
using EbayAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            if (newItem.CategoriesId == null)
                return Ok("Empty cat!");
            int newId = await _itemService.InsertItem(seller.UserId,newItem);
            
            return Ok($"A new auction has been created successful for item with id : {newId}!");
        }

        [HttpPost("/category/{id}", Name = "GetItemsByCategory")]
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



    }
    
}