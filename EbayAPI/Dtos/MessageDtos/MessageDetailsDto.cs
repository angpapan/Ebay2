namespace EbayAPI.Dtos.MessageDtos;

public class MessageDetailsDto
{
    /// <summary>
    /// The user that sent the message
    /// </summary>
    [Required]
    public string UsernameFrom { get; set; }
    
    /// <summary>
    /// The user that receives the message
    /// </summary>
    [Required]
    public string UsernameTo { get; set; }
    
    /// <summary>
    /// Message subject
    /// </summary>
    [Required, StringLength(200)]
    public string Subject { get; set; }
    
    /// <summary>
    /// The message main text
    /// </summary>
    [Required, StringLength(4000)]
    public string Body { get; set; }
    
    /// <summary>
    /// The system time when the message was sent
    /// </summary>
    [Required]
    public DateTime TimeSent { get; set; }
    
    /// <summary>
    /// The datetime that receiver read the message.
    /// Null if the message have not been read.
    /// </summary>
    [Required]
    public DateTime? ReceiverRead { get; set; }
    
    /// <summary>
    /// The message id to which this message replies
    /// Null if it's not a reply message
    /// </summary>
    public int? ReplyForId { get; set; } 
    
    /// <summary>
    /// The message subject to which this message replies
    /// Null if it's not a reply message
    /// </summary>
    public string? ReplyForSubject { get; set; } 
}