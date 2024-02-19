
using API.Helper;
using Microsoft.AspNetCore.Mvc;
//remember the controller in .net is actually a class handling the work
namespace API.Controllers;

//here the controller will replaced by User
//here we are using controller base for non vew mvc but for other we use Controller directly
    
    
    //here now the LogUserActivity can be applied to controller or endpoint to
    //but anyway we are authenticating the user in LogUserActivity so we can make it like this also
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[Controller]")]

    public class BaseApiController : ControllerBase
    {

    }