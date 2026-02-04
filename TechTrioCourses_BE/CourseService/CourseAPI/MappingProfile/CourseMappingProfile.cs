using AutoMapper;
using CourseAPI.DTOs;
using TechTrioCourses.Shared.Enums;
using static CourseAPI.DTOs.Response.CourseResponse;
using CourseAPI.DTOs.Request;
using CourseAPI.DTOs.Response;

namespace CourseAPI.MappingProfile
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<CreateCourseRequest, Models.Course>();
            CreateMap<UpdateCourseRequest, Models.Course>()
.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Models.Course, CourseResponse>();
             
        }
    }
}
