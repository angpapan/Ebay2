using System.Text.Json;
using EbayAPI.Dtos;
using EbayAPI.Dtos.BidDtos;
using EbayAPI.Helpers;
using EbayAPI.Helpers.Authorize;
using EbayAPI.Models;
using EbayAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EbayAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("bids")]
    public class BidController : ControllerBase
    {
        private readonly BidService _bidService;

        public BidController(BidService bidService)
        {
            _bidService = bidService;
        }

        /// <summary>
        /// Creates a new bid for an item
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        [HttpPost("/createBid", Name = "CreateNewBid")]
        [Authorize]
        public async Task<IActionResult> CreateNewBid([FromBody] BidRequest bid)
        {
            
            await _bidService.SetNewBid(bid, (User?)HttpContext.Items["User"]);
            return Ok($"Your bid has been assigned. BID => Item:{bid.ItemId} Amount:{bid.Amount}!");
            
        }
        
        
        /// <summary>
        /// Gets a paged list of all the items which the user
        /// making the request have bidded for
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("bidder-list", Name = "BidderItemsList")]
        [Authorize(Roles.User)]
        public async Task<List<UserBidInfoDto>> BidderItemList([FromQuery] BidderItemListQueryParameters dto)
        {
            User? bidder = (User?) HttpContext.Items["User"];
            PagedList<UserBidInfoDto> items = await _bidService.GetUserBidInfo(dto, bidder!);

            var metadata = new
            {
                items.TotalCount,
                items.PageSize,
                items.CurrentPage,
                items.TotalPages,
                items.HasNext,
                items.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            return items;
        }
        
        
    }
}

