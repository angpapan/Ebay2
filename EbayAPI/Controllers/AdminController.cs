using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using EbayAPI.Dtos;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using EbayAPI.Models;
using EbayAPI.Services;
using EbayAPI.Data;
using EbayAPI.Helpers;
using EbayAPI.Helpers.Authorize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EbayAPI.Controllers
{
    [ApiController]
    [Helpers.Authorize.Authorize(Roles.Administrator)]
    [Produces("application/json")]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly EbayAPIDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AdminService _adminService;
        private readonly RecommendationService _rec;

        public AdminController(ILogger<AdminController> logger,
            EbayAPIDbContext dbContext,
            IMapper mapper,
            RecommendationService rec,
            AdminService adminService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _adminService = adminService;
            _rec = rec;
        }
        
        /// <summary>
        /// Enable an eixsting user
        /// </summary>
        /// <param name="username">The user's username to enable</param>
        /// <returns></returns>
        [HttpPost("confirmUser", Name = "ConfirmUser")]
        public async Task<IActionResult> EnableUser(string username)
        {
            await _adminService.EnableUser(username);
            return Ok($"User {username} has been enabled !");
        }
        
        /// <summary>
        /// Get a paged list of all users
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("users", Name = "GetUsersList")]
        public async Task<List<UserReduced>> UsersList([FromQuery] UserListQueryParameters parameters)
        {
            PagedList<User> users = await _adminService.GetAllUsers(parameters);
            
            var metadata = new
            {
                users.TotalCount,
                users.PageSize,
                users.CurrentPage,
                users.TotalPages,
                users.HasNext,
                users.HasPrevious
            };
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
            
            return _mapper.Map<List<UserReduced>>(users);
        }
        
        /// <summary>
        /// Extract the specified items data to xml or json format
        /// </summary>
        /// <param name="item_ids">Item ids to generate xml/json</param>
        /// <param name="type">Can be "xml" or "json". Default is xml.</param>
        /// <returns></returns>
        [HttpGet("extract", Name = "ExtractItemInfo")]
        [Produces("text/plain")]
        public async Task<string> Extract([Required, FromQuery] List<int> item_ids, string type = "xml")
        {
            if (type != "xml" && type != "json")
            {
                type = "xml";
            }

            return await _adminService.ExtractItemInfo(item_ids, type);
        }
        
        /// <summary>
        /// Import the data from the data-xmls. It will auto generate users, bids etc.
        /// Import skips already imported items, in case of re-import
        /// </summary>
        /// <param name="start">The xml file to to start importing</param>
        /// <param name="end">The xml file to stop importing</param>
        /// <returns></returns>
        [HttpPost("import-xmls", Name = "ImportDataSet")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportData(int start = 0, int end = 39)
        {
            if (end > 39 || end < 0 || start > 39 || start < 0)
            return BadRequest("Invalid arguments"); 
            
            await _adminService.ImportXmlData(start, end, true);
            return Ok("Data Imported successfully!");
        }
        
        /// <summary>
        /// Calculate and store to database the latent matrices
        /// of users and items based on user bids. If values have
        /// been already calculated the old values will be deleted. 
        /// </summary>
        /// <returns></returns>
        [HttpGet("factorize")]
        [AllowAnonymous]
        public async Task<IActionResult> Factorization()
        {
            _rec.InitNew();
            _rec.Factorize();
            
            return Ok();
        }

        /// <summary>
        /// Gets recommendations for a user.
        /// To be used only for testing by admin.
        /// </summary>
        /// <param name="id">The user id to get the recommendations for</param>
        /// <param name="num">The number of recommended items</param>
        /// <returns></returns>
        [HttpGet("recommendations/{id}")]
        [AllowAnonymous]
        public async Task<List<Item>> Recommend(int id, int num = 6)
        {
            List<int>? items = await _rec.GetRecommendations(id, num);

            if (items == null)
            {
                return new List<Item>();
            }

            return _dbContext.Items
                .Where(i => items!.Contains(i.ItemId))
                .ToList();
        }
    }
    
}