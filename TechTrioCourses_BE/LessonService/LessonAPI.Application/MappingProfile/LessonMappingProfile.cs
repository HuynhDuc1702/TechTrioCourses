using AutoMapper;
using LessonAPI.Application.DTOs;
using TechTrioCourses.Shared.Enums;
using static LessonAPI.Application.DTOs.Response.LessonResponse;
using LessonAPI.Application.DTOs.Request;
using LessonAPI.Application.DTOs.Response;
using LessonAPI.Domain.Entities;

namespace LessonAPI.Application.MappingProfile
{
    public class LessonMappingProfile : Profile
    {
        public LessonMappingProfile()
        {
            CreateMap<CreateLessonRequest, Lesson>();
            CreateMap<UpdateLessonRequest, Lesson>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Lesson, LessonResponse>();

        }
    }
}
