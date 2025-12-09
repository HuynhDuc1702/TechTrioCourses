using AutoMapper;
using LessonAPI.DTOs;
using LessonAPI.Enums;
using static LessonAPI.DTOs.Response.LessonResponse;
using LessonAPI.DTOs.Request;
using LessonAPI.DTOs.Response;

namespace LessonAPI.MappingProfile
{
    public class LessonMappingProfile : Profile
    {
        public LessonMappingProfile()
        {
            CreateMap<CreateLessonRequest, Models.Lesson>();
            CreateMap<UpdateLessonRequest, Models.Lesson>()
.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Models.Lesson, LessonResponse>();
             
        }
    }
}
