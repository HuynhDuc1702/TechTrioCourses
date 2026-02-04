using AutoMapper;
using UserAPI.DTOs.Request.UserLesson;
using UserAPI.DTOs.Response.UserLesson;
using UserAPI.Models;

namespace UserAPI.MappingProfile
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
