namespace API.Entities;

public class Message
{
    public int Id {get;set;}
    public int SendId {get;set;}
    public string SenderUserName {get;set;}
    public AppUser Sender {get;set;}
    public int ReceipientId {get;set;} 
    public string ReceipientUserName {get;set;}
    public AppUser Reciepient {get;set;}
    public string Content {get;set;}
    public DateTime? DateRead {get;set;}
    
    public DateTime MessageSent {get;set;}=DateTime.UtcNow;
    public bool SenderDeleted {get;set;}
    public bool ReceipientDeleted {get;set;}

}