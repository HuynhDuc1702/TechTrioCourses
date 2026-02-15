using AutoMapper;
using UserAPI.Application.DTOs.Request.UserSelectedChoice;
using UserAPI.Application.DTOs.Response.UserSelectedChoice;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.MappingProfile
{
    public class UserSelectedChoiceMappingProfile : Profile
    {
        public UserSelectedChoiceMappingProfile()
        {
            // UserSelectedChoice mappings
            CreateMap<UserSelectedChoice, UserSelectedChoiceResponse>();
            CreateMap<CreateUserSelectedChoiceRequest, UserSelectedChoice>();
            CreateMap<UpdateUserSelectedChoiceRequest, UserSelectedChoice>()
                         .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserSelectedChoice, UserSelectedChoiceResponse>();
        }
    }
}
