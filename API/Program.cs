using System.Text;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);
//Services can be in any sequence but the request pipeline should be in sequence as one run after other

//services are basically the db service and other services 

//the app.json contain secrete info mainly so keep it in gitignore
builder.Services.AddControllers();
// Add services to the container.
//for discriptions refer extensions folder classes
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityExtensions(builder.Configuration);


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

