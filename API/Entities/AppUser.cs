using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser{

    // here the convention will run and Id will ne taken as primary key but for implicit is given by
    //[Key] like this
    public int Id { get; set; }
    public string UserName {get; set;}

    public byte[] PasswordHash{get;set;}
    public byte[] PasswordSalt{get;set;}
}