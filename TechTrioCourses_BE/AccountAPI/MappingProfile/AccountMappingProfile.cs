using AccountAPI.Models;
using AutoMapper;
using AccountAPI.Enums;
using AccountAPI.DTOs.Request;
using AccountAPI.DTOs.Response;

namespace AccountAPI.MappingProfile
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

           
        }
    }
}
