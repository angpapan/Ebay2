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
using EbayAPI.Dtos.MessageDtos;
using EbayAPI.Helpers;
using EbayAPI.Helpers.Authorize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EbayAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("category")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly EbayAPIDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger,
            EbayAPIDbContext dbContext,
            IMapper mapper,
            CategoryService categoryService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Return a list of all available categories
        /// </summary>
        /// <returns></returns>
        [HttpGet("", Name = "GetCategories")]
        [AllowAnonymous]
        public async Task<List<CategoryDto>> GetCategories()
        {
            return await _categoryService.GetCategoriesAsync();
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// Return a list of all available Locations
        /// </summary>
        /// <returns></returns>
        [HttpGet("/locations", Name = "GetLocations")]
        [AllowAnonymous]
        public async Task<List<string>> GetLocations()
        {
            return await _dbContext.Items
                .Select(i => i.Country)
                .Distinct()
                .ToListAsync();
        }
    }
}