using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService : ITokenService
{
    //as JWT signature check happen server side only so symmetric single key enoughh
    //symmetric key both enc and decy same key
    private readonly SymmetricSecurityKey _key;

    //here the Iconfiguration is nothing just 
    //getting the element from the configuration file key for jwt signature (header,payload(claim), signature)
    
    public TokenService(IConfiguration config){
        _key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
    }

    public string CreateToken(AppUser user){
        
        //these are the claims in payload that user is having and saying that i am this when he send the jwt back to server
        var claims=new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId,user.UserName)
        };

        //signing the signature part
        var creds=new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);


        //here now we are creating the token description that how our token is going to be in real by add the entities
        var tokenDescriptor=new SecurityTokenDescriptor{
            Subject=new ClaimsIdentity(claims),
            Expires=DateTime.Now.AddDays(7),
            SigningCredentials=creds
        };


        var tokenHandler=new JwtSecurityTokenHandler();
        //creating the
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);



    }
}