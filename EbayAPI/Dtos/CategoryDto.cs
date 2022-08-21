using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class CategoryDto
{
    [Required] public int CategoryId { get; set; }
    [Required] public string Name { get; set; }
}