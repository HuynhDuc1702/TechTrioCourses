using AutoMapper;
using UserAPI.Application.DTOs.Request.UserCourse;
using UserAPI.Application.DTOs.Response.UserCourse;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.MappingProfile
{
    public class UserCourseMappingProfile : Profile
    {
        public UserCourseMappingProfile()
        {
            // Map CreateUserCourseRequest -> UserCourse
            CreateMap<CreateUserCourseRequest, UserCourse>()
                     .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Progress, opt => opt.Ignore())
                .ForMember(dest => dest.EnrolledAt, opt => opt.Ignore())
               .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
          .ForMember(dest => dest.CompletedAt, opt => opt.Ignore());

            // Map UserCourse -> UserCourseResponse
            CreateMap<UserCourse, UserCourseResponse>();
        }
    }
}
