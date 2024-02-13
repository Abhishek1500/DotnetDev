using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;


//here the entity framwork will create the Photos in db and and take care of the relationship also 
[Table("Photos")]
public class Photo
{
    public int Id {get; set;}
    public string Url {get; set;}
    public bool IsMain {get; set;}
    public string PublicId {get; set;}
//here we are adding this for creating the 2 way mapping kind of thing because other wise Db will keep userid as nullable
    public int AppUserid {get;set;}
    public AppUser AppUser {get; set;}
}

//using foreignkey

/*
    public int AppUserid {get;set;}
    [ForeignKey("AppUserid")]
    public AppUser AppUser {get; set;}
*/