using System.Text;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using API.Extensions;
using API.Middleware;

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

//these are middleware


app.UseMiddleware<ExceptionMiddleware>();

//cors is for client side browser safety look into extensions for more info
app.UseCors(builder=> builder.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("http://localhost:4200"));


//use to show the exceptions ist a middleware inbuild in this program.cs but in production it wont show
//app.UseDeveloperExceptionPage();
//implemented with if of builder.Environment.idDevelopment()

app.UseAuthentication();
//once the user is authenticated they can go to authenticated endpoint 
app.UseAuthorization();

app.MapControllers();
//seeding the data
using var scope=app.Services.CreateScope();
var services=scope.ServiceProvider;
try{
    var context=services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}catch(Exception ex){
    var logger =services.GetService<ILogger<Program>>();
    logger.LogError(ex,"An error occoured during migration");
}

app.Run();

//here the error handling middleware that we talk about above 
//present above all middleware therefore if error occour in any of the lower
//middleware it pass back to above error pipeline