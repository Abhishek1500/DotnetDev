
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class IdentityServiceExtension {

    public static IServiceCollection AddApplicationService(this IServiceCollection service,IConfiguration config){
        
        //here we are we are adding the service that tell that say if jwt token came how server should check it is good token
            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options=>{
                options.TokenValidationParameters=new TokenValidationParameters{
                    //check and look validity on the basis of issuer signing key in token and the key we give in second line otherwise all jwt accepted
                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(config["TokenKey"])),
                    //validate issuer to validate the third part kind of thing may be not sure
                    ValidateIssuer=false,
                    ValidateAudience=false
                };
            });
            return service;
    }
}