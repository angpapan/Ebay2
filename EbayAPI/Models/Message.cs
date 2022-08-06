namespace EbayAPI.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        [Required, StringLength(200)]
        public string Subject { get; set; }
        
        [Required, StringLength(4000)]
        public string Body { get; set; }
        
        [Required]
        public DateTime TimeSent { get; set; } = DateTime.Now;

        [Required] 
        public bool SenderDelete { get; set; } = false;
        
        [Required] 
        public bool ReceiverDelete { get; set; } = false;

        public DateTime? ReceiverRead { get; set; } = null;

        public int? ReplyForId { get; set; } = null; 
        
        [Required] 
        public int SenderId { get; set; }
        
        [Required] 
        public int ReceiverId { get; set; }
        
        [JsonIgnore, ForeignKey("SenderId")]
        public virtual User Sender { get; set; }
        [JsonIgnore, ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; }
        
        public virtual Message? ReplyFor { get; set; }
    }
}