using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    private readonly DataContext _context;
    public BuggyController(DataContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("auth")]

    public ActionResult<string> GetSecret()
    {
        return "Secret text";
    }

    [HttpGet("not-found")]

    public ActionResult<AppUser> GetNotFound()
    {
        AppUser thing=null;
        thing.ToString();
        return thing;
    }

    [HttpGet("server-error")]

    public ActionResult<string> GetServerError()
    {
        var thing=_context.Users.Find(-1);
        return thing.ToString();
        
    }

    [HttpGet("bad-request")]

    public ActionResult<String> GetBadRequest()
    {
        return BadRequest("This is a bad request");
    }

    
    

}