using AutoMapper;
using CourseAPI.Application.DTOs;
using TechTrioCourses.Shared.Enums;
using CourseAPI.Domain.Entities;
using static CourseAPI.Application.DTOs.Response.CourseResponse;
using CourseAPI.Application.DTOs.Request;
using CourseAPI.Application.DTOs.Response;

namespace CourseAPI.Application.MappingProfile
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<CreateCourseRequest, Course>();
            CreateMap<UpdateCourseRequest, Course>()
.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Course, CourseResponse>();
             
        }
    }
}
