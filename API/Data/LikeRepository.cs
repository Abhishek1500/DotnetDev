using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helper;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikeRepository : ILikesRepository
{
    private readonly DataContext _context;
    public LikeRepository(DataContext context){
        _context=context;
    }
    public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
    {
        return await _context.Likes.FindAsync(sourceUserId,targetUserId);
    }

    public async Task<PageList<LikeDto>> GetUserLikes(LikesParams likeParams)
    {
        var users=_context.Users.OrderBy(u=>u.UserName).AsQueryable();
        var likes=_context.Likes.AsQueryable();

        if(likeParams.Predicate=="liked"){
            likes=likes.Where(like=>like.SourceUserId==likeParams.UserId);
            users=likes.Select(like=>like.TargetUser);
        }
        if(likeParams.Predicate=="likedBy"){
            likes=likes.Where(like=>like.TargetUserId==likeParams.UserId);
            users=likes.Select(like=>like.SourceUser);
        }
        var likedUsers= users.Select(user=>new LikeDto{
            UserName=user.UserName,
            KnownAs=user.knownAs,
            Age=user.DateOfBirth.CalculateAge(),
            PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain).Url,
            City=user.City,
            Id=user.Id
        });

        return await PageList<LikeDto>.CreateAsync(likedUsers,likeParams.PageNumber,likeParams.PageSize);

    }

    public async Task<AppUser> GetUserWithLikes(int userId)
    {
        return await _context.Users.Include(x=>x.LikedUser)
        .FirstOrDefaultAsync(x=>x.Id==userId);
    }
}