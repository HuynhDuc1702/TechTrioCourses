using AutoMapper;
using UserAPI.DTOs.Request.UserQuizzeResult;
using UserAPI.DTOs.Response.UserQuizzeResult;
using UserAPI.Models;

namespace UserAPI.MappingProfile
{
    public class UserQuizzeResultMappingProfile : Profile
    {
        public UserQuizzeResultMappingProfile()
        {
            // UserQuizzeResult mappings
            CreateMap<UserQuizzeResult, UserQuizzeResultResponse>();
            CreateMap<CreateUserQuizzeResultRequest, UserQuizzeResult>();
            CreateMap<UpdateUserQuizzeResultRequest, UserQuizzeResult>()
              .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserQuizzeResult, UserQuizzeResultResponse>();
        }
    }
}
