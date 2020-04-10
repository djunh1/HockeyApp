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

            //STEP 2 - CLOUD STORAGE - updating on API (Next: controller)
            CreateMap<UserForUpdateDto, User>();

            //Step 10 CLOUD STORAGE , can update controller
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>()
             .ForMember(m => m.SenderPhotoUrl, opt => opt
                .MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
             .ForMember(m => m.RecipientPhotoUrl, opt => opt
                .MapFrom(u => u.Recipient.Photos.FirstOrDefault( p => p.IsMain).Url));
        }
    }
}