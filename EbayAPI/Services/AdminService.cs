using EbayAPI.Data;
using EbayAPI.Dtos;
using EbayAPI.Helpers;
using EbayAPI.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EbayAPI.Services;
public class AdminService
{
    private readonly AppSettings _appSettings;
    private readonly EbayAPIDbContext _dbContext;
    private readonly IMapper _mapper;

    public AdminService(IOptions<AppSettings> appSettings, EbayAPIDbContext dbContext, IMapper mapper)
    {
        _appSettings = appSettings.Value;
        _dbContext = dbContext;
        _mapper = mapper;
    }


    // enabling a newly registered user
    public async Task EnableUser(string username)
    {
        User user = _dbContext.Users
            .Where(u => u.Username == username)
            .SingleOrDefault();
        
        if (user == null)
            throw new BadHttpRequestException("Invalid username.");

        user.Enabled = true;
        
        // δεν χρειάζονται
        // _dbContext.Users.Attach(user);
        // _dbContext.Entry(user).Property(x => x.Enabled).isModified = true;
        await _dbContext.SaveChangesAsync();
    }

    // return all users with details so admin can have access on them
    public async Task<PagedList<User>> GetAllUsers(UserListQueryParameters parameters)
    {
        IQueryable<User> users = _dbContext.Users;
        
        if (parameters.Search != null)
        {
            users = users.Where(u => u.Username.Contains(parameters.Search) || u.FirstName.Contains(parameters.Search) ||
                             u.LastName.Contains(parameters.Search) || u.Email.Contains(parameters.Search));
        }

        QuerySortHelper<User> usr = new QuerySortHelper<User>();
        users = usr.QuerySort(users, parameters.OrderBy);

        PagedList<User> userPage =
            PagedList<User>.ToPagedList(users, parameters.PageNumber, parameters.PageSize);

        return userPage;
    }


}