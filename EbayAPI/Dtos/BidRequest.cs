using EbayAPI.Models;

namespace EbayAPI.Dtos;

public class BidRequest
{
    public int ItemId {get; set;}
    public decimal Amount {get; set;}
}