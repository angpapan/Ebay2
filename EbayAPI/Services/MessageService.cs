using EbayAPI.Data;
using EbayAPI.Dtos;
using EbayAPI.Helpers;
using EbayAPI.Models;
using AutoMapper;
using EbayAPI.Dtos.MessageDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EbayAPI.Services;
public class MessageService
{
    private readonly AppSettings _appSettings;
    private readonly EbayAPIDbContext _dbContext;
    private readonly IMapper _mapper;

    public MessageService(IOptions<AppSettings> appSettings, EbayAPIDbContext dbContext, IMapper mapper)
    {
        _appSettings = appSettings.Value;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    
    /// <summary>
    /// creates a message from sender to receiver user
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="sender">The user sending the message</param>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public async Task SendMessageAsync(SendMessageDto dto, User? sender)
    {
        // TODO check if the two users had a successful transaction
        
        if (sender == null)
        {
            throw new UnauthorizedAccessException("Please login to send a message.");
        }

        if (sender.Username == dto.ReceiverUsername)
        {
            throw new NotSupportedException("It's not yet possible to send a message to yourself.");
        }

        User? receiver = await _dbContext.Users
            .Where(u => u.Username == dto.ReceiverUsername)
            .SingleOrDefaultAsync();

        if (receiver == null)
        {
            throw new KeyNotFoundException($"User {dto.ReceiverUsername} does not exist.");
        }
            
        Message msg = _mapper.Map<Message>(dto);
        msg.SenderId = sender.UserId;
        msg.ReceiverId = receiver.UserId;

        await _dbContext.AddAsync(msg);
        await _dbContext.SaveChangesAsync();
    }
    
    
    
    /// <summary>
    /// Finds all the inbox messages for a user
    /// </summary>
    /// <param name="user">The user to get inbox of</param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public async Task<PagedList<Message>> GetUserInboxAsync(User? user, QueryPagingParameters parameters)
    {
        if (user == null)
        {
            throw new UnauthorizedAccessException("Please login to view your inbox.");
        }

        IQueryable<Message> messages = _dbContext.Messages
            .Include(m => m.Sender)
            .Where(m => m.ReceiverId == user.UserId && m.ReceiverDelete == false)
            .OrderByDescending(m => m.TimeSent);

        PagedList<Message> messagePage =
            PagedList<Message>.ToPagedList(messages, parameters.PageNumber, parameters.PageSize);
        
        

        return messagePage;
    }
    
    
    /// <summary>
    /// Finds all the outbox messages for a user
    /// </summary>
    /// <param name="user">The user to get outbox of</param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public async Task<PagedList<Message>> GetUserOutboxAsync(User? user, QueryPagingParameters parameters)
    {
        if (user == null)
        {
            throw new UnauthorizedAccessException("Please login to view your outbox.");
        }

        IQueryable<Message> messages = _dbContext.Messages
            .Include(m => m.Receiver)
            .Where(m => m.SenderId == user.UserId && m.SenderDelete == false)
            .OrderByDescending(m => m.TimeSent);

        PagedList<Message> messagePage =
            PagedList<Message>.ToPagedList(messages, parameters.PageNumber, parameters.PageSize);

        return messagePage;
    }
    
    
    /// <summary>
    /// Gets a message
    /// </summary>
    /// <param name="user">The user making the request</param>
    /// <param name="id">The message to be retrieved</param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<Message> GetMessageByIdAsync(User? user, int id)
    {
        if (user == null)
        {
            throw new UnauthorizedAccessException("Please login to read the message.");
        }

        Message? message = await _dbContext.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Include(m => m.ReplyFor)
            .Where(m => m.MessageId == id)
            .SingleOrDefaultAsync();

        if (message == null)
        {
            throw new KeyNotFoundException("The message does not exist.");
        }
        
        if (message.SenderId != user.UserId && message.ReceiverId != user.UserId)
        {
            throw new UnauthorizedAccessException("You cannot read this message.");
        }

        if (message.SenderId == user.UserId && message.SenderDelete == true ||
            message.ReceiverId == user.UserId && message.ReceiverDelete == true)
        {
            throw new KeyNotFoundException("This message has been deleted.");
        }

        return message;
    }
    
    /// <summary>
    /// Deletes the specified message for the user making the request
    /// </summary>
    /// <param name="user">The user making the request</param>
    /// <param name="id">The message to be deleted</param>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task DeleteMessageForUserAsync(User? user, int id)
    {
        if (user == null)
        {
            throw new UnauthorizedAccessException("Please login to delete the message.");
        }

        Message? message = await _dbContext.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => m.MessageId == id)
            .SingleOrDefaultAsync();

        if (message == null)
        {
            throw new KeyNotFoundException("The message does not exist.");
        }

        if (message.SenderId == user.UserId)
        {
            message.SenderDelete = true;
        } 
        else if (message.ReceiverId == user.UserId)
        {
            message.ReceiverDelete = true;
        }
        else
        {
            throw new UnauthorizedAccessException("You cannot delete this message.");
        }

        await _dbContext.SaveChangesAsync();
    }


}