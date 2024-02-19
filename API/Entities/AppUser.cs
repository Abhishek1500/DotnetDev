using System.ComponentModel.DataAnnotations;
using API.Extensions;

namespace API.Entities;

public class AppUser{

    // here the convention will run and Id will ne taken as primary key but for implicit is given by
    //[Key] like this
    public int Id { get; set; }
    public string UserName {get; set;}

    public byte[] PasswordHash{get;set;}
    public byte[] PasswordSalt{get;set;}

    public DateTime DateOfBirth {get; set;}

    public string knownAs{get;set;}
    public DateTime Created {get;set;}=DateTime.UtcNow;

    public DateTime LastActive {get; set;}=DateTime.UtcNow;

    public string Gender {get; set;}

    public string Introduction {get; set;}

    public string LookingFor {get;set;}

    public string Intrests {get; set;}
    public string City {get; set;}
    public string Countery {get; set;}
     public List<Photo> Photos {get;set;}=new List<Photo>();

     public List<UserLike> LikedByUser {get;set;}
     public List<UserLike> LikedUser {get;set;}
    //here the auto mapper will be  good enough to know that GetAge is for Age but remember that Get Age name and Get is required
   //   public int GetAge(){
   //      return DateOfBirth.CalculateAge();
   //   }


}

