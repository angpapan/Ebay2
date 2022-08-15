using AutoMapper;
using EbayAPI.Data;
using EbayAPI.Dtos;
using EbayAPI.Helpers;
using EbayAPI.Models;
using EbayAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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


        [HttpGet("/search", Name = "SearchItems")]
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

        [HttpPost("upload-images"), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload([FromForm] ItemAddition dto)
        {
            var a = dto;
            var formCollection = await Request.ReadFormAsync();


            //var folderName = Path.Combine("StaticFiles", "Images");
            //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            //if (files.Any(f => f.Length == 0))
            //{
            //    return BadRequest();
            //}
            //foreach (var file in files)
            //{
            //    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            //    var fullPath = Path.Combine(pathToSave, fileName);
            //    var dbPath = Path.Combine(folderName, fileName); //you can add this path to a list and then return all dbPaths to the client if require
            //    using (var stream = new FileStream(fullPath, FileMode.Create))
            //    {
            //        file.CopyTo(stream);
            //    }
            //}
            return Ok("All the files are successfully uploaded.");
        }


    }
    
}