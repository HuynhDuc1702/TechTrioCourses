using AutoMapper;
using UserAPI.DTOs.Request;
using UserAPI.DTOs.Response;
using UserAPI.Enums;
using UserAPI.Models;

namespace UserAPI.MappingProfile
{
    public class UserCourseMappingProfile : Profile
    {
        public UserCourseMappingProfile()
      {
            // Map CreateUserCourseRequest -> UserCourse
    CreateMap<CreateUserCourseRequest, UserCourse>()
     .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (UserCourseStatus)src.Status))
         .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EnrolledAt, opt => opt.Ignore())
        .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
        .ForMember(dest => dest.CompletedAt, opt => opt.Ignore());

  // Map UpdateUserCourseRequest -> UserCourse (for updating existing user course)
       CreateMap<UpdateUserCourseRequest, UserCourse>()
  .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

   // Map UserCourse -> UserCourseResponse
          CreateMap<UserCourse, UserCourseResponse>();
        }
    }
}
