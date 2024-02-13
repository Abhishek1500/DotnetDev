using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;
    //Ilogger is used to log  the exception
    //_env here is to check the environment
    public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,
    IHostEnvironment env)
    {
        _env=env;
        _logger=logger;
        _next=next;
    }

//InvokeAsync is the same name to use 
//we are going to tell framework that this is middleware so it aspect InvokeAsync
//it is used to invoke next middleware using next
//here the HttpContext give us access to http request
    public async Task InvokeAsync(HttpContext context){
        //here the flow is like that from here the _next will pass request to next middleware so if at any point error occour and it is not handled
        //then this try catch catch it
        try{
            await _next(context);
        }catch(Exception ex){
            //logging on terminal
            _logger.LogError(ex,ex.Message);
            //here we need to specify yhe type of content in response we want to return
            // we dont do that in controllers because they know by default that json to be sent
            //but outof controller it is required
            context.Response.ContentType="application/json";
            context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;

            //last one is may be or may noet be there therfore ? is applied optional
            var response=_env.IsDevelopment()
            ? new ApiException(context.Response.StatusCode,ex.Message,ex.StackTrace?.ToString())
            :new ApiException(context.Response.StatusCode,ex.Message,"Internal Server error");
            
            //options for response ?????
            var options=new JsonSerializerOptions{PropertyNamingPolicy=JsonNamingPolicy.CamelCase};

            var json=JsonSerializer.Serialize(response,options);
            await context.Response.WriteAsync(json);
        }
    }
}