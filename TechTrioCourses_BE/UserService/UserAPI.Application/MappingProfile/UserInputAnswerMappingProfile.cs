using AutoMapper;
using UserAPI.Application.DTOs.Request.UserInputAnswer;
using UserAPI.Application.DTOs.Response.UserInputAnswer;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.MappingProfile
{
    public class UserInputAnswerMappingProfile : Profile
    {
        public UserInputAnswerMappingProfile()
        {
            // UserInputAnswer mappings
            CreateMap<UserInputAnswer, UserInputAnswerResponse>();
            CreateMap<CreateUserInputAnswerRequest, UserInputAnswer>();
            CreateMap<UpdateUserInputAnswerRequest, UserInputAnswer>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
