using API.DTO;
using API.Entities;
using API.Helper;

namespace API.Interfaces;

public interface IuserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> getUsersAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<PageList<MemberDto>> GetMemberAsync(UserParams userParams);
    Task<MemberDto> GetMemberAsync(string username);
}