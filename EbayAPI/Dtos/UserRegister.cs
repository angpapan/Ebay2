namespace EbayAPI.Dtos;

public class UserRegister
{
    [Required] public string Username { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string VerifyPassword { get; set; }
    [Required] public string FirstName {get; set;}
    [Required] public string LastName {get; set;}
    [Required] public string Email {get; set;}
    [Required] public string PhoneNumber {get; set;}
    [Required] public string Street {get; set;}
    [Required] public int StreetNumber {get; set;}
    [Required] public string City {get; set;}
    [Required] public string PostalCode {get; set;}
    [Required] public string Country {get; set;}
    [Required] public string VATNumber { get; set; }
}