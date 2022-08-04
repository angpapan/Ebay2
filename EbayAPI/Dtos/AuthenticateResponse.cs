using EbayAPI.Helpers.Authorize;
using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class AuthenticateResponse
{
    [Required] public string Username { get; set; }
    /// <summary>
    /// The logged in user role. Could be "admin" or "user".
    /// </summary>
    [Required] public string Role { get; set; }
    /// <summary>
    /// True if user is validated by an administrator. Otherwise false.
    /// </summary>
    [Required] public bool Enabled { get; set; }
    /// <summary>
    /// The generated JWT Token
    /// </summary>
    [Required] public string Token { get; set; }

    public AuthenticateResponse(User user, string token)
    {
        Username = user.Username;
        Enabled = user.Enabled;
        switch (user.RoleId)
        {
            case 1:
                Role = Roles.AdministratorStr;
                break;
            case 2:
                Role = Roles.UserStr;
                break;
            default:
                // should never get here
                throw new KeyNotFoundException("User does not have a role!");
        }
        Token = token ?? throw new ArgumentNullException(nameof(token));
    }
}