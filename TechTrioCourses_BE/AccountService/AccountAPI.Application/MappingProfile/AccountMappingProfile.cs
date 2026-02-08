using AccountAPI.Application.DTOs.Request;
using AccountAPI.Application.DTOs.Response;
using AccountAPI.Domain.Entities;

using AutoMapper;
using TechTrioCourses.Shared.Enums;

namespace AccountAPI.Application.MappingProfile
{
    public class AccountMappingProfile: Profile
        
    {
        public AccountMappingProfile() {
            // Map RegisterRequest -> Account
            CreateMap<RegisterRequest, Account>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // hash later
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatusEnum.Disable)) // Set to Disable until OTP verified
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<Account, AccountResponse>();

           
        }
    }
}
