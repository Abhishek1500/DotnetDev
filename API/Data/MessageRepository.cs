using API.Controllers;
using API.DTO;
using API.Entities;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public MessageRepository(DataContext context,IMapper mapper){
        _context=context;
        _mapper=mapper;
    }

    public void AddMessage(Message message)
    {
        _context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        _context.Messages.Remove(message);
    }

    public async Task<Message> GetMessage(int id)
    {
        return await _context.Messages.FindAsync(id);
    }


    public async Task<PageList<MessageDto>> GetMessageForUser(MessageParams messageParams)
    {
        var query=_context.Messages
        .OrderByDescending(x=>x.MessageSent).AsQueryable();

        query=messageParams.Container switch{
           "Inbox"=> query.Where(u=>u.ReceipientUserName==messageParams.Username&&u.ReceipientDeleted==false),
           "Outbox"=>query.Where(u=>u.SenderUserName==messageParams.Username&&u.SenderDeleted==false),
           //default
           _=>query.Where(u=>u.ReceipientUserName==messageParams.Username&& u.ReceipientDeleted==false&&u.DateRead==null)
        };
        var messages=query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

        return await PageList<MessageDto>.CreateAsync(messages,messageParams.PageNumber,messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string receipientUserName)
    {
        var messages=await _context.Messages
        .Include(u=>u.Sender).ThenInclude(p=>p.Photos).
        Include(u=>u.Reciepient).ThenInclude(p=>p.Photos)
        .Where(
            m=>m.ReceipientUserName==currentUserName
            &&m.ReceipientDeleted==false
            &&m.SenderUserName==receipientUserName
            || m.ReceipientUserName==receipientUserName&&
            m.SenderDeleted==false&&
            m.SenderUserName==currentUserName
        ).OrderBy(m=>m.MessageSent).ToListAsync();

        var unreadMessage=messages.Where(m=>m.DateRead==null&&
        m.ReceipientUserName==currentUserName).ToList();
        if(unreadMessage.Any()){
            foreach(var message in unreadMessage){
                message.DateRead=DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }

        return _mapper.Map<IEnumerable<MessageDto>>(messages);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync()>0;
    }
}