using AutoMapper;
using UserAPI.Application.DTOs.Request.UserLesson;
using UserAPI.Application.DTOs.Response.UserLesson;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.MappingProfile
{
    public class UserLessonMappingProfile : Profile
    {
        public UserLessonMappingProfile()
        {
            // Map CreateUserLessonRequest -> UserLesson
            CreateMap<CreateUserLessonRequest, UserLesson>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedAt, opt => opt.Ignore());

            // Map UserLesson -> UserLessonResponse
            CreateMap<UserLesson, UserLessonResponse>();
        }
    }
}
