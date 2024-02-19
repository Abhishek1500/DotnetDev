using API.DTO;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikeController : BaseApiController
{
    private readonly IuserRepository _userRepository;
    private readonly ILikesRepository _likesRepository;
    
    public LikeController(IuserRepository userRepository,ILikesRepository likesRepository){
        _likesRepository=likesRepository;
        _userRepository=userRepository;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string username){
        var sourceUserId=User.GetUserId();
        var likedUser=await _userRepository.GetUserByUsernameAsync(username);
        var sourceUser=await _likesRepository.GetUserWithLikes(sourceUserId);

        if(likedUser==null)return NotFound();
        if(sourceUser.UserName==username) return BadRequest("Youcant like yoursef");
        var userLike=await _likesRepository.GetUserLike(sourceUserId,likedUser.Id);
        if(userLike!=null) return BadRequest("You already liked the user");
        userLike=new Entities.UserLike{
            SourceUserId=sourceUserId,
            TargetUserId=likedUser.Id
        };
        sourceUser.LikedUser.Add(userLike);
        if(await _userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Failed to like user");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LikeDto>>> getUserLikes(string predicate){
        var users=await _likesRepository.GetUserLikes(predicate,User.GetUserId());
    }
}