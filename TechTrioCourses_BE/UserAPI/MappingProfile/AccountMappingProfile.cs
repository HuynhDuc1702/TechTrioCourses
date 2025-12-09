using AutoMapper;
using UserAPI.DTOs.Request;
using UserAPI.DTOs.Response;
using UserAPI.Enums;
using UserAPI.Models;

namespace UserAPI.MappingProfile
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Map CreateUserRequest -> User
            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (UserRoleEnum)src.Role))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // Map UpdateUserRequest -> User (for updating existing user)
            CreateMap<UpdateUserRequest, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Map User -> UserResponse
            CreateMap<User, UserResponse>();
        }
    }
}
