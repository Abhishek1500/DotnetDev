using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions{
    public static string GetUsername(this ClaimsPrincipal user){

        return user.FindFirst(ClaimTypes.Name)?.Value;

    }

    public static int GetUserId(this ClaimsPrincipal user){
        var b=user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(b);
    }
}