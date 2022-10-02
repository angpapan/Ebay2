using EbayAPI.Data;
using EbayAPI.Dtos;
using EbayAPI.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace EbayAPI.Services;
public class CategoryService
{
    private readonly EbayAPIDbContext _dbContext;
    private readonly IMapper _mapper;

    public CategoryService(EbayAPIDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        List<Category> categories = await _dbContext.Categories
            .OrderBy(c=>c.Name)
            .ToListAsync();

        return _mapper.Map<List<CategoryDto>>(categories);
    }
    
    public async Task<List<CategoryDto>> GetTopCategoriesAsync(int num)
    {
        List<Category> categories = await _dbContext.Categories
            .Include(c => c.CategoryItems)
            .OrderByDescending(c => c.CategoryItems.Count)
            .Take(num)
            .ToListAsync();

        return _mapper.Map<List<CategoryDto>>(categories);
    }

}