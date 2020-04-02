using System.Linq;
using AutoMapper;
using HockeyApp.API.dtos;
using HockeyApp.API.Models;

namespace HockeyApp.API.helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Defining the photo URL property, and were the information comes from
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, 
                            opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url));
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl,
                            opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url));
            CreateMap<Photo, PhotosForDetailedDto>();
        }
    }
}