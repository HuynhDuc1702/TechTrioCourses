using CourseAPI.Application.DTOs.Response;
using CourseAPI.Application.Interfaces.IExternalServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TechTrioCourses.Shared.Dtos.User;

namespace CourseAPI.Infrastructure.ExternalServices
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

        public async Task<UserResponse?> GetUserByIdAsync(Guid id)
        {
            string cacheKey = $"User_{id}";

            if (_cache.TryGetValue(cacheKey, out UserResponse? cached))
            {
                return cached;
            }

            try
            {
                var response = await _httpClient.GetFromJsonAsync<UserResponse>(
                    $"api/users/{id}");

                if (response != null)
                {
                    _cache.Set(cacheKey, response, TimeSpan.FromMinutes(30));
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch User {UserId}", id);
                return null;
            }
        }
        public async Task<Dictionary<Guid, string>> GetCreatorNamesAsync(IEnumerable<Guid> UserIds)
        {
            var ids = UserIds.Distinct().ToList();
            if (!ids.Any())
                return new();

            var cacheKey = $"users_{string.Join("_", ids.OrderBy(x => x))}";

            if (_cache.TryGetValue(cacheKey, out Dictionary<Guid, string>? cached))
            {
                _logger.LogInformation("Using cached users");
                return cached!;
            }

            try
            {
                var response = await _httpClient
                    .PostAsJsonAsync("api/users/get-by-ids", ids);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to fetch users. Status: {Status}",
                        response.StatusCode);
                    return new();
                }

                var users = await response.Content
                    .ReadFromJsonAsync<List<UserResponse>>()
                    ?? new();

                var result = users.ToDictionary(c => c.Id, c => c.FullName);

                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(30));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                return new();
            }
        }

        public async Task PopulateCreatorNameAsync(List<CourseResponse> courses)
        {
            var UserIds = courses
                .Where(c => c.CreatorId.HasValue)
                .Select(c => c.CreatorId!.Value)
                .Distinct();

            var UserMap = await GetCreatorNamesAsync(UserIds);

            foreach (var course in courses)
            {
                if (course.CreatorId.HasValue &&
                    UserMap.TryGetValue(course.CreatorId.Value, out var name))
                {
                    course.CreatorName = name;
                }
            }
        }


    }
}
