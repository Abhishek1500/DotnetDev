using System.Text;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


//Services can be in any sequence but the request pipeline should be in sequence as one run after other

//the app.json contain secrete info mainly so keep it in gitignore


// Add services to the container.


//services are basically the db service and other services 
builder.Services.AddControllers();
//HERE THE CORS IS ADDED FOR SAFE DATA TRANSFER FROM SERVER TO angular app server

//cors is a browser security feature to prevent your browser to download nasty data from different origin
// therefore it is our api server duty to send the header for saying that the server at client side angular that i am okay to use 
//if no do client can download dirty js
builder.Services.AddCors();


//adding Token service
//here we have 3 possibility->transit very short living live just till we use;
//scoped live till response transfer or controller thing;  
//Singletone is used to long lived till app stops running
//here even if without Implementing and directly using Token Service is also good
//but testing thingh
builder.Services.AddScoped<ITokenService,TokenService>();
//here we are we are adding the service that tell that say if jwt token came how server should check it is good token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options=>{
    options.TokenValidationParameters=new TokenValidationParameters{
        //check and look validity on the basis of issuer signing key in token and the key we give in second line otherwise all jwt accepted
        ValidateIssuerSigningKey=true,
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8
        .GetBytes(builder.Configuration["TokenKey"])),
        //validate issuer to validate the third part kind of thing may be not sure
        ValidateIssuer=false,
        ValidateAudience=false
    };

});


//here we are adding the dabcontext class to our program.cs so that we can use that context file 
builder.Services.AddDbContext<DataContext>(opt=>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseCors(builder=> builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseAuthentication();
//once the user is authenticated they can go to authenticated endpoint 
app.UseAuthorization();

app.MapControllers();

app.Run();

