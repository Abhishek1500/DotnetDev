using System.Security.Cryptography;
using System.Text;
using API.Controllers;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace APi.Controller;

public class AccountController : BaseApiController{
    DataContext _context;
    public AccountController(DataContext context){
        _context=context;
    }

    [HttpPost("register")]//Post: api/account/register

    //by convention the parameters take the querystring as value
    //say we send api/Addcount/register?username=dam&password=password and query name should be same
    public async Task<ActionResult<AppUser>> Register(string username,string password){
        //here the purpose of using this using is that let you created the object of hashmac
        //then it will be deleted by garbage collector if it is not refrenced but we dont know when because we delete the refrence not object
        // what using will do once the refrence die the object also die because of dispose methon inside its parent class it  is disposable class remember I dont know about other classes
        using var hmac=new HMACSHA512();

        var user=new AppUser{
            UserName=username,
            PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
            //here this hamc key is randomly generated 
            PasswordSalt=hmac.Key
        };
        //here we are just telling ef to add this to db it is not adding anything
        _context.Users.Add(user);
        //this will add the data
        await _context.SaveChangesAsync();
        return user;
    }
}