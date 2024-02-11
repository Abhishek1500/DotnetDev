using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;

public class UsersController : AppUser
{
    public readonly DataContext _context;

    //here in this userController constructor the DataContext context instance is created with http request 
    //and once the work is done it got scoped out so no need to pass and oce the controller done with its 
    //work i.e. reseponce instance die

    //here the parameters for the given constructor is obj of services that we added before kind of
    public UsersController(DataContext context)
    {
        _context = context;
    }
    //cant use var it is accessible only in local in webapi
    //here ActionResult is a return type of a //controller// method way of saying but read indepth to know more
    // [HttpGet]
    // public ActionResult<IEnumerable<AppUser>> GetUsers()
    // {
    //     var users=_context.Users.ToList();
    //     //here the as we are sending the users as responce
    //     return users;
    // }

    // consider the above commented code first and read comments this is its async version
    // now in async the Task is return

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsersAsync() //this is naming convention
    {
        //toListAsync is coming from entity framework
        var users = await _context.Users.ToListAsync();
        //here the as we are sending the users as responce
        return users;
    }

    //once we make our code async the request is pass to another thread known delegates
    [HttpGet("{id}")] // /api/users/2
    public async Task<ActionResult<AppUser>> GetUserAsync(int id)
    {
        AppUser user = await _context.Users.FindAsync(id);
        // here sending single user as responce
        return user;
    }
}