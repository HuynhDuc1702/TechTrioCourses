using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using TechTrioCourses.Shared.Dtos.Lesson;
using TechTrioCourses.Shared.Enums;
using UserAPI.Application.Interfaces.IExternalServices;

namespace UserAPI.Infrastructure.ExternalServices
{
    public class LessonApiClient : ILessonApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LessonApiClient> _logger;
        private readonly IMemoryCache _cache;

        public LessonApiClient(
            IHttpClientFactory httpClientFactory,
            ILogger<LessonApiClient> logger,
            IMemoryCache cache)
        {
            _httpClient = httpClientFactory.CreateClient("LessonAPI");
            _logger = logger;
            _cache = cache;
        }

        public async Task<int> GetLessonCountByCourseIdAsync(Guid courseId)
        {
            var cacheKey = $"CourseLessons_{courseId}";


            if (_cache.TryGetValue(cacheKey, out int cachedCount))
            {
                return cachedCount;
            }

            try
            {
                var lessons = await _httpClient
                            .GetFromJsonAsync<List<LessonResponse>>(
                             $"api/lessons/course/{courseId}")
                             ?? new List<LessonResponse>();

                var count = lessons.Count(l => l.Status == PublishStatusEnum.Published);


                _cache.Set(cacheKey, lessons, TimeSpan.FromMinutes(30));

                return count;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(
                ex,
                "Failed to fetch lessons for course {CourseId}",
                 courseId);

                return 0;
            }
        }
    }
}
