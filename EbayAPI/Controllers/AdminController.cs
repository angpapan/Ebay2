using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
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

        public AdminController(ILogger<AdminController> logger,
            EbayAPIDbContext dbContext,
            IMapper mapper,
            AdminService adminService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _adminService = adminService;
        }

        [HttpPost("confirmUser", Name = "ConfirmUser")]
        public async Task<IActionResult> EnableUser(string username)
        {
            await _adminService.EnableUser(username);
            return Ok($"User {username} has been enabled !");
        }
        
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
        
        [HttpGet("extract", Name = "ExtractItemInfo")]
        [Produces("text/plain")] // TODO return plain text or json ????
        public async Task<string> Extract([Required, FromQuery] List<int> item_ids, string type = "xml")
        {
            if (type != "xml" && type != "json")
            {
                type = "xml";
            }

            return await _adminService.ExtractItemInfo(item_ids, type);


        }

        [HttpPost("import-xmls", Name = "ImportDataSet")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportData(int number = 1)
        {
            if (number > 40 || number < 1)
                return BadRequest("Please give a number between 1 and 40"); 
            
            await _adminService.ImportXmlData(number);
            return Ok("Data Imported successfully!");
        }
    }
}