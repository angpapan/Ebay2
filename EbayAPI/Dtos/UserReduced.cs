using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class UserReduced
{
    [Required] public string Username { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName {get; set;}
    [Required] public string Email {get; set;}
    [Required] public bool Enabled { get; set; }
    [Required] public DateTime DateCreated {get; set; }
}