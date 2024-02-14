using API.DTO;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helper;

public class AutoMapperProfile :Profile {

    //here we create the mapping between our dto and models or dto other stuff if we want
    public AutoMapperProfile(){
        //mapping
        //telling the mapper how to initialize value of PhotoUrl 
              //  from      To
        CreateMap<AppUser,MemberDto>()
        .ForMember(dest=>dest.PhotoUrl,
        opt=>opt.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url)).
        ForMember(dest=>dest.age,opt=>opt.MapFrom(src=>src.DateOfBirth.CalculateAge()));
        CreateMap<Photo,PhotoDto>();
        CreateMap<MemberUpdateDto,AppUser>();

    }

}