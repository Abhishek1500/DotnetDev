namespace API.DTO;

public class MemberDto{
        public int Id { get; set; }
    public string UserName {get; set;}

    public int age {get; set;}

    public string PhotoUrl{get;set;}
    public string knownAs{get;set;}
    public DateTime Created {get;set;}//=DateTime.UtcNow;

    public DateTime LastActive {get; set;}//=DateTime.UtcNow;

    public string Gender {get; set;}

    public string Introduction {get; set;}

    public string LookingFor {get;set;}

    public string Intrests {get; set;}
    public string City {get; set;}
    public string Countery {get; set;}
    public List<PhotoDto> Photos {get;set;}//=new List<Photo>();

}