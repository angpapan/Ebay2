using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EbayAPI.Helpers;
using EbayAPI.Models;
using Microsoft.Extensions.Options;

namespace EbayAPI.Services;

public static class GlobalService
{
    public static string ComputeSha256Hash(string rawData)  
    {  
        // Create a SHA256   
        using (SHA256 sha256Hash = SHA256.Create())  
        {  
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));  
  
            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();  
            for (int i = 0; i < bytes.Length; i++)  
            {  
                builder.Append(bytes[i].ToString("x2"));  
            }  
            return builder.ToString();  
        }  
    }
}