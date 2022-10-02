using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AutoMapper;
using EbayAPI.Models;
using EbayAPI.Services;
using EbayAPI.Data;
using EbayAPI.Dtos.MessageDtos;
using EbayAPI.Helpers;
using EbayAPI.Helpers.Authorize;

namespace EbayAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [Route("message")]
    public class MessageController : ControllerBase
    {
        private readonly EbayAPIDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MessageService _messageService;

        public MessageController(
            EbayAPIDbContext dbContext,
            IMapper mapper,
            MessageService messageService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _messageService =  messageService;
        }
        
        /// <summary>
        /// Sends a message between two users
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("send", Name = "SendMessage")]
        public async Task<IActionResult> SendMessage(SendMessageDto dto)
        {
            User? sender  = (User?)HttpContext.Items["User"];
            
            await _messageService.SendMessageAsync(dto, sender);
            return Ok($"Message sent successfully!");
        }
        
        /// <summary>
        /// Gets the user's received messages in pages
        /// </summary>
        [HttpGet("inbox", Name = "Inbox")]
        public async Task<List<MessageListDto>> Inbox([FromQuery] MessageQueryParameters parameters)
        {
            User? sender  = (User?)HttpContext.Items["User"];
            
            PagedList<Message> msgs = await _messageService.GetUserInboxAsync(sender, parameters);
            
            var metadata = new
            {
                msgs.TotalCount,
                msgs.PageSize,
                msgs.CurrentPage,
                msgs.TotalPages,
                msgs.HasNext,
                msgs.HasPrevious
            };
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
            
            List<MessageListDto> inbox = _mapper.Map<List<MessageListDto>>(msgs);
            return inbox;
        }
        
        
        /// <summary>
        /// Gets the user's sent messages in pages
        /// </summary>
        [HttpGet("outbox", Name = "Outbox")]
        public async Task<List<MessageListDto>> Outbox([FromQuery] MessageQueryParameters parameters)
        {
            User? user  = (User?)HttpContext.Items["User"];
            
            PagedList<Message> msgs = await _messageService.GetUserOutboxAsync(user, parameters);
            
            var metadata = new
            {
                msgs.TotalCount,
                msgs.PageSize,
                msgs.CurrentPage,
                msgs.TotalPages,
                msgs.HasNext,
                msgs.HasPrevious
            };
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
            
            List<MessageListDto> outbox = _mapper.Map<List<MessageListDto>>(msgs);
            return outbox;
        }
        
        /// <summary>
        /// Gets a message details
        /// </summary>
        /// <param name="id">The message id to be read</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "ReadMessage")]
        public async Task<MessageDetailsDto> ReadMessage(int id)
        {
            User? user  = (User?)HttpContext.Items["User"];
            
            Message msg = await _messageService.GetMessageByIdAsync(user, id);
            
            // the receiver reads the message for the first time
            if (msg.Receiver.UserId == user!.UserId && msg.ReceiverRead == null)
            {
                msg.ReceiverRead = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }

            MessageDetailsDto message = _mapper.Map<MessageDetailsDto>(msg);

            return message;
        }
        
        
        /// <summary>
        /// Deletes a message for a user
        /// </summary>
        /// <param name="id">The message id to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteMessage")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            User? user  = (User?)HttpContext.Items["User"];
            
            await _messageService.DeleteMessageForUserAsync(user, id);
            
            return Ok("Message deleted successfully.");
        }
        
        /// <summary>
        /// Get the number of received, new and sent messages
        /// </summary>
        [HttpGet("stats", Name = "MessageStatistics")]
        public async Task<MessageStatsDto> MessageStats()
        {
            User? user  = (User?)HttpContext.Items["User"];
            
            return await _messageService.GetUserStatsAsync(user);
        }
        
        /// <summary>
        /// Gets the number of unread inbox messages for the
        /// user making the request
        /// </summary>
        /// <returns></returns>
        [HttpGet("check-new", Name = "CheckForNewMessages")]
        public async Task<int> CheckNew()
        {
            User? user  = (User?)HttpContext.Items["User"];
            return await _messageService.CheckForNew(user!);
        }
    }
}