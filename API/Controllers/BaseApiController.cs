
using Microsoft.AspNetCore.Mvc;
//remember the controller in .net is actually a class handling the work
namespace API.Controllers;

//here the controller will replaced by User
//here we are using controller base for non vew mvc but for other we use Controller directly

    [ApiController]
    [Route("api/[Controller]")]

    public class BaseApiController : ControllerBase
    {

    }