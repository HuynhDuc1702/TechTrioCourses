using AutoMapper;
using UserAPI.Application.DTOs.Request.UserQuizzeResult;
using UserAPI.Application.DTOs.Response.UserQuizzeResult;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.MappingProfile
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
