using System.Security.Cryptography;
using System.Text;
using API.Controllers;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DTO;
using API.Interfaces;
namespace APi.Controller;
//DTO=>Data transfer object basically do one thingh it encapsulate the data and transfer it from one app to another
//and also used as select as linq to create new obj with required fields
public class AccountController : BaseApiController{
    DataContext _context;
    ITokenService _tokenService;
    public AccountController(DataContext context,ITokenService tokenService){
        _context=context;
        _tokenService=tokenService;
    }

    [HttpPost("register")]//Post: api/account/register

    //by convention the parameters take the querystring as value
    //say we send api/Addcount/register?username=dam&password=password and query name should be same
    // for validation is done in DTO the API Contoller first look for the validation in Dto
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){
        
        if (await UserExists(registerDto.UserName)) return BadRequest("UserName is Taken");
        
        
        //here the purpose of using this using is that let you created the object of hashmac
        //then it will be deleted by garbage collector if it is not refrenced but we dont know when because we delete the refrence not object
        // what using will do once the refrence die the object also die because of dispose methon inside its parent class it  is disposable class remember I dont know about other classes
        using var hmac=new HMACSHA512();

        var user=new AppUser{
            UserName=registerDto.UserName.ToLower(),
            PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            //here this hamc key is randomly generated 
            PasswordSalt=hmac.Key
        };
        //Console.WriteLine(hmac.Key.SequenceEqual(user.PasswordSalt));
        //here we are just telling ef to add this to db it is not adding anything
        _context.Users.Add(user);
        //this will add the data
        await _context.SaveChangesAsync();
        return new UserDto{
            Username=user.UserName,
            Token=_tokenService.CreateToken(user)
        };
    }


    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
        var user=await _context.Users.SingleOrDefaultAsync(x=>x.UserName==loginDto.Username.ToLower());
        if(user==null) return Unauthorized("Invalid User Name");
        using var hmac=new HMACSHA512(user.PasswordSalt);
        var computedhash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        if(!computedhash.SequenceEqual(user.PasswordHash)) return Unauthorized("not valid password");
        Console.WriteLine(user.UserName);
        return new UserDto{
            Username=user.UserName,
            Token=_tokenService.CreateToken(user)
        };
    }


    private async Task<bool> UserExists(string username){
            return await _context.Users.AnyAsync(x=>x.UserName==username.ToLower());
    }

}