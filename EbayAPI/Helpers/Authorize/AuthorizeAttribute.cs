#pragma warning disable CS1591
using Microsoft.AspNetCore.Authorization;

namespace EbayAPI.Helpers.Authorize;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EbayAPI.Models;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private int[] AllowedRoles;

    public AuthorizeAttribute(params int[] roles)
    {
        AllowedRoles = roles;
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        User? user = (User?) context.HttpContext.Items["User"];
        
        // check for [AllowAnonymous]
        bool allowAnonymous = context.ActionDescriptor.EndpointMetadata
            .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
        
        if (allowAnonymous)
            return;
        
        // not logged in
        if (user == null)
            throw new UnauthorizedAccessException("Please login to gain access.");

        // not enabled
        if(!user.Enabled)
            throw new UnauthorizedAccessException("You are not verified yet. Please wait " +
                                                  "for an administrator to approve your account.");
        
        // logged in but doesn't have permission to access
        if (AllowedRoles.Length != 0 && !AllowedRoles.Contains(user.RoleId))
            throw new UnauthorizedAccessException("Unauthorized access.");
    }
}