using AccountAPI.Application.DTOs.Request;
using AccountAPI.Application.Interfaces.IExternalServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using TechTrioCourses.Shared.Dtos.User;
using TechTrioCourses.Shared.Enums;

namespace AccountAPI.Infrastructure.ExternalServices
{
    public class UserApiClient : IUserApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserApiClient> _logger;
        private readonly IMemoryCache _cache;

        public UserApiClient(
        IHttpClientFactory httpClientFactory,
        ILogger<UserApiClient> logger,
        IMemoryCache cache)
        {
            _httpClient = httpClientFactory.CreateClient("UserAPI");
            _logger = logger;
            _cache = cache;
        }

        public async Task<UserResponseForRegister?> RegisterUser(Guid accountId, RegisterRequest request)
        {
            try
            {
                var createUserRequest = new
                {
                    AccountId = accountId,
                    request.FullName,
                    request.AvatarUrl,
                };

                var content = new StringContent(
                JsonSerializer.Serialize(createUserRequest),
                          Encoding.UTF8,
                            "application/json");

                var response = await _httpClient.PostAsync("/api/Users", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserResponseForRegister>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CreateUserViaAPI] Error: {ex.Message}");
                return null;
            }
        }

        public async Task<UserResponseForLogin> GetUserByAccountIdFromUserAPI(Guid accountId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/Users/by-account/{accountId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var userDto = JsonSerializer.Deserialize<UserResponseForLogin>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return userDto;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetUserByAccountIdFromUserAPI] Error: {ex.Message}");
                return null;
            }
        }

        public string ConvertRoleToEnumName(UserRoleEnum role)
        {
            return role switch
            {
                UserRoleEnum.Admin => "Admin",
                UserRoleEnum.Student => "Student",
                UserRoleEnum.Instructor => "Instructor",
                _ => "Student"
            };
        }
    }
}
