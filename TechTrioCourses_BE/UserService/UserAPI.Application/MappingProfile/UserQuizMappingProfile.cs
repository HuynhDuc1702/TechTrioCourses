using AutoMapper;
using UserAPI.Application.DTOs.Request.UserQuiz;
using UserAPI.Application.DTOs.Response.UserQuiz;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.MappingProfile
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
