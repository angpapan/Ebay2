using Microsoft.AspNetCore.Mvc;
using EbayAPI.Dtos;
using EbayAPI.Services;
using EbayAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EbayAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("category")]
    public class CategoryController : ControllerBase
    {
        private readonly EbayAPIDbContext _dbContext;
        private readonly CategoryService _categoryService;

        public CategoryController(
            EbayAPIDbContext dbContext,
            CategoryService categoryService)
        {
            _dbContext = dbContext;
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
        
        /// <summary>
        /// Get a list of top categories based on number of items
        /// </summary>
        /// <param name="num">The number of top categories to return</param>
        /// <returns></returns>
        [HttpGet("top", Name = "GetTopCategories")]
        [AllowAnonymous]
        public async Task<List<CategoryDto>> GetTopCategories(int num = 10)
        {
            return await _categoryService.GetTopCategoriesAsync(num);

        }
    }
}
