namespace EbayAPI.Dtos.MessageDtos;

public class MessageStatsDto
{
    /// <summary>
    /// Total undeleted received messages
    /// </summary>
    [Required]
    public int InboxTotal { get; set; }
    
    /// <summary>
    /// Total unread received messages
    /// </summary>
    [Required]
    public int InboxNew { get; set; }
    
    /// <summary>
    /// Total undeleted sent messages
    /// </summary>
    [Required]
    public int OutboxTotal { get; set; }
    
}