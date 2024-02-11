namespace API.DTO;
//the thing with Dto is that let say we pass elelements in API body json format like
// username and password then it should match the entities name here written then just after that it will bind them
//cases is not the case but name is
public class RegisterDto{
    //properties should be in capital letter 
    //talking about properties not fields
    public string UserName{get;set;}
    public string Password{get; set;}

}