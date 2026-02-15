using LessonAPI.Application.DTOs.Response;
using LessonAPI.Application.Interfaces.IExternalServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TechTrioCourses.Shared.Dtos.Category;
using TechTrioCourses.Shared.Dtos.Course;

namespace LessonAPI.Infrastructure.ExternalServices
{
    public class CourseApiClient : ICourseApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CourseApiClient> _logger;
        private readonly IMemoryCache _cache;

        public CourseApiClient(
            IHttpClientFactory httpClientFactory,
            ILogger<CourseApiClient> logger,
            IMemoryCache cache)
        {
            _httpClient = httpClientFactory.CreateClient("CourseAPI");
            _logger = logger;
            _cache = cache;
        }

        public async Task<CourseResponse?> GetCourseByIdAsync(Guid id)
        {
            string cacheKey = $"Course_{id}";

            if (_cache.TryGetValue(cacheKey, out CourseResponse? cached))
            {
                return cached;
            }

            try
            {
                var response = await _httpClient.GetFromJsonAsync<CourseResponse>(
                    $"api/courses/{id}");
                

                if (response != null)
                {
                    _cache.Set(cacheKey, response, TimeSpan.FromMinutes(30));
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch course {CourseId}", id);
                return null;
            }
        }
       


    }
}
