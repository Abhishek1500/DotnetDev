using API.Entities;

namespace API.Interfaces;
//here the interface is created the main reason for creating thew interface for the
//testing purpose by convention so that the testing could be easy
//now we can implement the services using such interfaces
public interface ITokenService
{
    string CreateToken(AppUser user);
}