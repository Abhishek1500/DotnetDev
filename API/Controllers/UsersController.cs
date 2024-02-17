using System.Security.Claims;
using API.Data;
using API.DTO;
using API.Entities;
using API.Extensions;
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
    private readonly IPhotoService _photoService;
    //here in this userController constructor the DataContext context instance is created with http request 
    //and once the work is done it got scoped out so no need to pass and oce the controller done with its 
    //work i.e. reseponce instance die

    //here the parameters for the given constructor is obj of services that we added before kind of
    public UsersController(IuserRepository userRepository,IMapper mapper,IPhotoService photoService)
    {
        _userRepository = userRepository;
        _mapper=mapper;
        _photoService=photoService;
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
        var users = await _userRepository.GetMemberAsync();
        
        return Ok(users);
        //here the user in IEnumerable of AppUser therefore such a way
        // var userstoReturn=_mapper.Map<IEnumerable<MemberDto>>(user);
        // return userstoReturn.ToList();
        //here the as we are sending the users as responce
      //here when we send data we send the open flat data that is ref to ref due to include and to save it userd mapper
      //basically json open it and heart attack came.  
    }


    //once we make our code async the request is pass to another thread known delegates
    [HttpGet("{username}")] // /api/users/2
    public async Task<ActionResult<MemberDto>> GetUserAsync(string username)
    {
        return await _userRepository.GetMemberAsync(username);
        // here sending single user as responce
        // var useroutput=_mapper.Map<MemberDto>(user);
        // return Ok(useroutput);
    }


    //here the User in this is Not the model or somthing it is System.Security.Claim principle
    //for accessing token and claims in it
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        if(user==null) return NotFound();
        _mapper.Map(memberUpdateDto,user);
        //error handling for save changes dont work
        //what save changes do is return the no. of changes it made (changes like git if no change in content it wont rewrite it)
        if(await _userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("failed to update user");
    }


    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file){
        var user=await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        if(user==null) return NotFound();
        //AddPhotoAsync
        var result=await _photoService.AddPhotoAsync(file);
        if(result.Error!=null) return BadRequest(result.Error.Message);
        var photo=new Photo{
            Url=result.SecureUrl.AbsoluteUri,
            PublicId=result.PublicId
        };

        if(user.Photos.Count==0){
            photo.IsMain=true;
        }
        user.Photos.Add(photo);
        if(await _userRepository.SaveAllAsync()) 
        return CreatedAtAction(nameof(GetUserAsync),
        new {username=user.UserName},
        _mapper.Map<PhotoDto>(photo));
        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user=await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        if(user==null) return NotFound();

        var photo=user.Photos.FirstOrDefault(x=>x.Id==photoId);

        if(photo==null)return NotFound();
        if(photo.IsMain) return BadRequest("Already user main photo");

        var currentMain=user.Photos.FirstOrDefault(x=>x.IsMain);
        if(currentMain!=null) currentMain.IsMain=false;
        photo.IsMain=true;


        if(await _userRepository.SaveAllAsync())return NoContent();
        return BadRequest("problem setting the photo as main photo");


    }


    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user= await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        var photo=user.Photos.FirstOrDefault(x=>x.Id==photoId);
        if(photo==null) return NotFound();
        if(photo.IsMain) return BadRequest("Cant delete the main photo");

        if(photo.PublicId!=null){
            var result=await _photoService.DeletePhotoAsync(photo.PublicId);
            if(result.Error!=null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);
        if(await _userRepository.SaveAllAsync())return Ok();
        return BadRequest("Probelm Deleteing the photos");

    }


}