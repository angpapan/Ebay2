namespace EbayAPI.Dtos.MessageDtos;

public class MessageInboxDto
{
    [Required]
    public int MessageId { get; set; }
    
    /// <summary>
    /// The user that sent the message
    /// </summary>
    [Required]
    public string UsernameFrom { get; set; }
    
    /// <summary>
    /// Message subject
    /// </summary>
    [Required, StringLength(200)]
    public string Subject { get; set; }
    
    /// <summary>
    /// The system time when the message was sent
    /// </summary>
    [Required]
    public DateTime TimeSent { get; set; }
    
    /// <summary>
    /// True if the user have already seen the message
    /// </summary>
    [Required]
    public bool isRead { get; set; }
}