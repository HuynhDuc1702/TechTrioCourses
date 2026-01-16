using AutoMapper;
using UserAPI.DTOs.Request.UserInputAnswer;
using UserAPI.DTOs.Response.UserInputAnswer;
using UserAPI.Models;

namespace UserAPI.MappingProfile
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
