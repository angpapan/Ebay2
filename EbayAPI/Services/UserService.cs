#pragma warning disable CS1591
using EbayAPI.Data;
using EbayAPI.Dtos;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using EbayAPI.Helpers;
using EbayAPI.Helpers.Authorize;
using EbayAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EbayAPI.Services;
public class UserService
{
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    private readonly AppSettings _appSettings;
    private readonly EbayAPIDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public UserService(IOptions<AppSettings> appSettings, EbayAPIDbContext dbContext, IMapper mapper)
    {
        _appSettings = appSettings.Value;
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _dbContext.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == GlobalService.ComputeSha256Hash(model.Password));

        if (user == null)
            throw new KeyNotFoundException("Username or password is incorrect");

        // authentication successful so generate jwt token
        var token = generateJwtToken(user);

        return new AuthenticateResponse(user, token);
    }

    public User GetById(int id)
    {
        return _dbContext.Users.Find(id);
    }

    public async Task<UserDetails> GetByUsernameAsync(string username, User? userRequests)
    {
        User user = _dbContext.Users
            .Where(u => u.Username == username)
            .SingleOrDefault();

        if (user == null)
            throw new KeyNotFoundException($"User {username} does not exist.");
        return (userRequests == null || userRequests.RoleId != Roles.Administrator)
            ? _mapper.Map<UserDetails>(_mapper.Map<UserDetailsUser>(user))
            : _mapper.Map<UserDetails>(user);
    }
    
    public async Task<bool> CheckUsernameExistenceAsync(string username)
    {
        User? user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Username == username);

        return user != null;

    }

    public async Task Register(UserRegister reg)
    {
        if (reg.Password != reg.VerifyPassword)
            throw new BadHttpRequestException("Password do not match.");

        reg.Password = GlobalService.ComputeSha256Hash(reg.Password);
        
        User usr = _mapper.Map<User>(reg);
        _dbContext.Users.Add(usr);
        await _dbContext.SaveChangesAsync();
    }

    
    
    
    
    
    
    // helper methods

    private string generateJwtToken(User user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}