using API.DTO;
using API.Entities;

namespace API.Interfaces;

public interface IuserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> getUsersAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<IEnumerable<MemberDto>> GetMemberAsync();
    Task<MemberDto> GetMemberAsync(string username);
}