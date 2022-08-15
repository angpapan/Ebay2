using EbayAPI.Data;
using EbayAPI.Dtos;
using EbayAPI.Helpers;
using EbayAPI.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EbayAPI.Services;
public class CategoryService
{
    private readonly AppSettings _appSettings;
    private readonly EbayAPIDbContext _dbContext;
    private readonly IMapper _mapper;

    public CategoryService(IOptions<AppSettings> appSettings, EbayAPIDbContext dbContext, IMapper mapper)
    {
        _appSettings = appSettings.Value;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        List<Category> categories = await _dbContext.Categories
            .Where(c => c.GenericId == null)
            .ToListAsync();

        return _mapper.Map<List<CategoryDto>>(categories);
    }

}