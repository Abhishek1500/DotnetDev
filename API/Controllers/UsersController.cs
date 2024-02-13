using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;

//now this will apply authorization required on each end point
// look here the thing is that [AllowAnonamous] is something that bypass every authorization
// therefore if you put it out for all and then put [authorization] for special then this authorization dont work
[Authorize]
public class UsersController : BaseApiController
{
    private readonly IuserRepository _userRepository;
    private readonly IMapper _mapper;

    //here in this userController constructor the DataContext context instance is created with http request 
    //and once the work is done it got scoped out so no need to pass and oce the controller done with its 
    //work i.e. reseponce instance die

    //here the parameters for the given constructor is obj of services that we added before kind of
    public UsersController(IuserRepository userRepository,IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper=mapper;
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

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsersAsync() //this is naming convention
    {
        //dut to action result it domt take ienumerable so use either to list or ok
        //toListAsync is coming from entity framework
        var user =await _userRepository.getUsersAsync();
        //here the user in IEnumerable of AppUser therefore such a way
        var userstoReturn=_mapper.Map<IEnumerable<MemberDto>>(user);
        return userstoReturn.ToList();
        //here the as we are sending the users as responce
        
    }


    //once we make our code async the request is pass to another thread known delegates
    [HttpGet("{id}")] // /api/users/2
    public async Task<ActionResult<MemberDto>> GetUserAsync(string username)
    {
        AppUser user = await _userRepository.GetUserByUsernameAsync(username);
        // here sending single user as responce
        var useroutput=_mapper.Map<MemberDto>(user);
        return Ok(useroutput);
    }
}