using API.DTO;
using API.Entities;
using API.Helper;

namespace API.Interfaces;

public interface ILikesRepository
{
    Task<UserLike> GetUserLike(int sourceUserId,int targetUserId);
    Task<AppUser> GetUserWithLikes(int userId);

    Task<PageList<LikeDto>> GetUserLikes(LikesParams likeparams);


    
}