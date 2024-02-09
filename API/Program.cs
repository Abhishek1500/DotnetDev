using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//the app.json contain secrete info mainly so keep it in gitignore

// Add services to the container.
//services are basically the db service and other services 
builder.Services.AddControllers();
//HERE THE CORS IS ADDED FOR SAFE DATA TRANSFER FROM SERVER TO angular app server

//cors is a browser security feature to prevent your browser to download nasty data from different origin
// therefore it is our api server duty to send the header for saying that the server at client side angular that i am okay to use 
//if no do client can download dirty js
builder.Services.AddCors();


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

// app.UseHttpsRedirection();

//  app.UseAuthorization();

app.MapControllers();

app.Run();
