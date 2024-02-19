using API.DTO;
using API.Entities;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

//in auto mapper if we are creating any function for entity and then mapping then the automapper require full entity for mapping

namespace API.Data;

public class UserRepository : IuserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public UserRepository(DataContext context,IMapper mapper){
        _context=context;
        _mapper=mapper;

    }

    //These are the example of querable mapper
    public async Task<MemberDto> GetMemberAsync(string username)
    {   //basically configprovider telling that it need to check mapper from where to maand how to map
        return await _context.Users
        .Where(x=>x.UserName==username)
        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
        .SingleOrDefaultAsync();
    }

    public async Task<PageList<MemberDto>> GetMemberAsync(UserParams userParams)
    {
        var query= _context.Users.AsQueryable();

        query=query.Where(u=>u.UserName!=userParams.CurrentUserName);
        query=query.Where(u=>u.Gender==userParams.Gender);

        var minDob=DateTime.Today.AddYears(-userParams.MaxAge-1);
        var maxDob=DateTime.Today.AddYears(-userParams.MinAge);
        query=query.Where(u=>u.DateOfBirth>=minDob&&u.DateOfBirth<=maxDob); 
        // .ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking();
        
        //switch on userParams
        query=userParams.OrderBy switch{
            "created"=>query.OrderByDescending(u=>u.Created),
         //default  
            _ => query.OrderByDescending(u=>u.LastActive) 
        };
        return await PageList<MemberDto>.CreateAsync(query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider),userParams.PageNumber,userParams.PageSize);
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

    //here the Include in this case will create the problem as it will keep the it in loop we dont wnat that 
    //look in at point where the data is taken we are using automapper there
    //here the link is not problem we can return it directly
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