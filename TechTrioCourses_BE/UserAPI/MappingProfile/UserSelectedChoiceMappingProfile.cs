using AutoMapper;
using UserAPI.DTOs.Request.UserSelectedChoice;
using UserAPI.DTOs.Response.UserSelectedChoice;
using UserAPI.Models;

namespace UserAPI.MappingProfile
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
