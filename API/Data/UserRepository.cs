using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository : IuserRepository
{
    private readonly DataContext _context;
    public UserRepository(DataContext context){
        _context=context;
    }
    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.
        Include(x=>x.Photos)
        .SingleOrDefaultAsync(x=>x.UserName==username);
    }

    public async Task<IEnumerable<AppUser>> getUsersAsync()
    {
        //here the ef dont put the data of mapping so to tell it to add we require this include
        return await _context.Users.
        Include(x=>x.Photos)
        .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync()>0;
    }

    public void Update(AppUser user)
    {
        _context.Entry(user).State=EntityState.Modified;
    }
}