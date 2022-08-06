using AutoMapper;
using EbayAPI.Dtos;
using EbayAPI.Dtos.MessageDtos;
using EbayAPI.Models;
using NuGet.Protocol;

namespace EbayAPI.Profiles;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<SendMessageDto, Message>();
        
        
        CreateMap<Message, MessageInboxDto>()
            .ForMember(dest => dest.UsernameFrom,
                opt =>
                    opt.MapFrom(src => src.Sender.Username))
            .ForMember(dest => dest.isRead,
                opt =>
                    opt.MapFrom(src => src.ReceiverRead != null));

        CreateMap<Message, MessageOutboxDto>()
            .ForMember(dest => dest.UsernameTo,
                opt =>
                    opt.MapFrom(src => src.Receiver.Username));
        
        CreateMap<Message, MessageDetailsDto>()
            .ForMember(dest => dest.UsernameFrom,
                opt =>
                    opt.MapFrom(src => src.Sender.Username))
            .ForMember(dest => dest.UsernameTo,
                opt =>
                    opt.MapFrom(src => src.Receiver.Username))
            .ForMember(dest => dest.ReplyForSubject,
                opt =>
                    opt.MapFrom(src => src.ReplyFor != null ? src.ReplyFor.Subject : null));
    }
}