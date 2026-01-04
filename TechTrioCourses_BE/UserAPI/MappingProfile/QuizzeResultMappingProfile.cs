using AutoMapper;
using UserAPI.DTOs.Request.QuizzeResult;
using UserAPI.DTOs.Response.QuizzeResult;
using UserAPI.Models;

namespace UserAPI.MappingProfile
{
    public class QuizzeResultMappingProfile : Profile
    {
        public QuizzeResultMappingProfile()
   {
          // QuizzeResult mappings
  CreateMap<QuizzeResult, QuizzeResultResponse>();
CreateMap<CreateQuizzeResultRequest, QuizzeResult>();
  CreateMap<UpdateQuizzeResultRequest, QuizzeResult>()
    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
     }
  }
}
