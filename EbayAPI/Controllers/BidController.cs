using AutoMapper;
using EbayAPI.Data;
using EbayAPI.Dtos;
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
        private readonly IMapper _mapper;
        private readonly EbayAPIDbContext _dbContext;
        private readonly ILogger<ItemService> _logger;
        private readonly BidService _bidService;

        public BidController(IMapper mapper, EbayAPIDbContext dbContext, ILogger<ItemService> logger,
            BidService bidService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
            _bidService = bidService;
        }

        
        [HttpPost("/createBid", Name = "CreateNewBid")]
        [Authorize]
        public async Task<IActionResult> CreateNewBid([FromBody] BidRequest bid)
        {
            
            switch (_bidService.SetNewBid(bid, (User)HttpContext.Items["User"]).Result )
            {case -1 :
                    return BadRequest($"Item with id {bid.ItemId} not found!");
            case -2 :
                return Ok("Bid doesnt assign because auction has been ended!");
            case -3 :
                return Ok("Bid doesnt assign. Your offer must be greater than last!");
            default:
                return Ok($"Your bid has been assigned. BID => Item:{bid.ItemId} Amount:{bid.Amount}!");
            
            }

        }

        [HttpGet("/myBids", Name = "GetMyBids")]
        [Authorize]
        public async Task<List<Bid>>? GetUsersBids()
        {
            return await _bidService.GetBids((User) HttpContext.Items["User"]);
        }
        
        
    }
}

