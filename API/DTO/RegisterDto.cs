using System.ComponentModel.DataAnnotations;

namespace API.DTO;
//the thing with Dto is that let say we pass elelements in API body json format like
// username and password then it should match the entities name here written then just after that it will bind them
//cases is not the case but name is
public class RegisterDto{
    //this binding and that parameter binding is the power of [APIController]
    //befor doing that we use to tell our controller where to look 
    //but it wont take care of validation
    /* eg
    Register([FromBody]RegisterDTO DTO])
    */
    //properties should be in capital letter 
    //talking about properties not fields
    [Required]
    public string UserName{get;set;}
    [Required]
    [StringLength(8,MinimumLength =4)]
    public string Password{get; set;}

}

//for the validation we can also do one thing make your model with that way