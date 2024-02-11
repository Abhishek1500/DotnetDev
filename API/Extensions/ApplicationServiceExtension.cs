using API.Interfaces;
using API.Services;
using API.Data;
using Microsoft.EntityFrameworkCore;
namespace API.Extensions;

public static class ApplicationServiceExtension{

     public static IServiceCollection AddIdentityExtensions(this IServiceCollection service,IConfiguration config){


        //HERE THE CORS IS ADDED FOR SAFE DATA TRANSFER FROM SERVER TO angular app server

        //cors is a browser security feature to prevent your browser to download nasty data from different origin
        // therefore it is our api server duty to send the header for saying that the server at client side angular that i am okay to use 
        //if no do client can download dirty js
        service.AddCors();


        //adding Token service
        //here we have 3 possibility->transit very short living live just till we use;
        //scoped live till response transfer or controller thing;  
        //Singletone is used to long lived till app stops running
        //here even if without Implementing and directly using Token Service is also good
        //but testing thingh
        service.AddScoped<ITokenService,TokenService>();

        //here we are adding the dabcontext class to our program.cs so that we can use that context file 
        service.AddDbContext<DataContext>(opt=>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });


        return service;
    }

}