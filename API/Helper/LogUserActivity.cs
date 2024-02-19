using System.Numerics;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helper;

public class LogUserActivity : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //here the API action got executed then the action filter come into picture
        //here if you want other way around use Executingcontext
        var resultContext=await next();
        if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;
        var username=resultContext.HttpContext.User.GetUsername();
        
        var repo=resultContext.HttpContext.RequestServices.GetRequiredService<IuserRepository>();
        var user=await repo.GetUserByUsernameAsync(username);
        Console.WriteLine(username);
        user.LastActive=DateTime.UtcNow;
        await repo.SaveAllAsync();


    }
}