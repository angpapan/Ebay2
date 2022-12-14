using Microsoft.AspNetCore.Mvc;
using EbayAPI.Dtos;
using EbayAPI.Models;
using EbayAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace EbayAPI.Controllers
{
    [ApiController]
    [Helpers.Authorize.Authorize]
    [Produces("application/json")]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        
        /// <summary>
        /// Gets a user's details. Details returned depend on the role of
        /// the user making the request. 
        /// </summary>
        /// <param name="username">The username to search</param>
        /// <response code="200">
        /// It returns the following info. If the user making the request is not an admin
        /// the non-required fields are null.
        /// </response>
        /// <response code="404">If the requested user is not found</response>
        [HttpGet("{username}", Name= "GetByUsername")]
        [AllowAnonymous]
        public async Task<UserDetails> GetByUsername(string username)
        {
            UserDetails details = await _userService.GetByUsernameAsync(username, (User?)HttpContext.Items["User"]);
            return details;
        }
        
        
        /// <summary>
        /// Checks if a username already exists
        /// </summary>
        /// <param name="username">The username to check</param>
        /// <response code="200">
        /// Returns true if username already exists, or false otherwise 
        /// </response>
        [HttpGet("exists/{username}", Name= "CheckUsernameExistence")]
        [AllowAnonymous]
        public async Task<bool> CheckUsernameExistence(string username)
        {
            return await _userService.CheckUsernameExistenceAsync(username);
        }
        
        /// <summary>
        /// Authenticates an existing user.
        /// </summary>
        [HttpPost("login", Name="Login")]
        [AllowAnonymous]
        public AuthenticateResponse Login(AuthenticateRequest req)
        {
            AuthenticateResponse response = _userService.Authenticate(req);
        
            return response;
        }
        
        
        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost("register", Name="Register")]
        [AllowAnonymous]
        public async Task<AuthenticateResponse> CreateUser(UserRegister reg)
        {
            await _userService.Register(reg);
            return _userService.Authenticate(new AuthenticateRequest(reg.Username, reg.Password));
        }
    }
}
