using API.Entities;

namespace API.DTO;

public class MessageDto
{
    public int Id {get;set;}
    public int SendId {get;set;}
    public string SenderUserName {get;set;}
    public string SenderPhtotoUrl {get;set;}

    public int ReceipientId {get;set;} 
    public string ReceipientUserName {get;set;}
    public string Content {get;set;}
    public string ReceipientPhtotoUrl {get;set;}

    public DateTime? DateRead {get;set;}
    
    public DateTime MessageSent {get;set;}
}