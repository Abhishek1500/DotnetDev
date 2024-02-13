using API.DTO;
using API.Entities;
using AutoMapper;

namespace API.Helper;

public class AutoMapperProfile :Profile {

    //here we create the mapping between our dto and models or dto other stuff if we want
    public AutoMapperProfile(){
        //mapping
        //telling the mapper how to initialize value of PhotoUrl 
        CreateMap<AppUser,MemberDto>()
        .ForMember(dest=>dest.PhotoUrl,
        opt=>opt.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url));

        CreateMap<Photo,PhotoDto>();

    }

}