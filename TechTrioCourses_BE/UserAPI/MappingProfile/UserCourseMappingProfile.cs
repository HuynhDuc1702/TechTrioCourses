using AutoMapper;
using UserAPI.DTOs.Request.UserCourse;
using UserAPI.DTOs.Response.UserCourse;
using UserAPI.Models;

namespace UserAPI.MappingProfile
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
