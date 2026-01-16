using AutoMapper;
using UserAPI.DTOs.Request.UserQuiz;
using UserAPI.DTOs.Response.UserQuiz;
using UserAPI.Models;

namespace UserAPI.MappingProfile
{
    public class UserQuizMappingProfile : Profile
    {
        public UserQuizMappingProfile()
        {
   // Map CreateUserQuizRequest -> UserQuiz
       CreateMap<CreateUserQuizRequest, UserQuiz>()
 .ForMember(dest => dest.Id, opt => opt.Ignore())
     .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
         .ForMember(dest => dest.FirstAttemptAt, opt => opt.Ignore())
       .ForMember(dest => dest.LastAttemptAt, opt => opt.Ignore())
       .ForMember(dest => dest.PassedAt, opt => opt.Ignore());

  

 // Map UserQuiz -> UserQuizResponse
   CreateMap<UserQuiz, UserQuizResponse>();
        }
}
}
