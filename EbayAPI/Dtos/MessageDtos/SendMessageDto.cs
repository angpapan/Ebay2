namespace EbayAPI.Dtos.MessageDtos;

public class SendMessageDto
{
    /// <summary>
    /// Message subject
    /// </summary>
    [Required, StringLength(200)]
    public string Subject { get; set; }
        
    /// <summary>
    /// Main message text
    /// </summary>
    [Required, StringLength(4000)]
    public string Body { get; set; }
        
    /// <summary>
    /// The message id that this message replies to
    /// </summary>
    public int? ReplyForId { get; set; } = null; 
        
    /// <summary>
    /// The user to receive the message
    /// </summary>
    [Required] public string ReceiverUsername { get; set; }
}