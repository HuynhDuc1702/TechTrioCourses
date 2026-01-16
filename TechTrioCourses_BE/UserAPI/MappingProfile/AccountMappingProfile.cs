using AutoMapper;
using UserAPI.DTOs.Request.User;
using UserAPI.DTOs.Response.User;
using TechTrioCourses.Shared.Enums;
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
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AccountId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.FullName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.FullName)))
                .ForMember(dest => dest.AvatarUrl, opt => opt.Condition(src => src.AvatarUrl != null))
                .ForMember(dest => dest.Role, opt => opt.Condition(src => src.Role.HasValue));

            // Map User -> UserResponse
            CreateMap<User, UserResponse>();
        }
    }
}
