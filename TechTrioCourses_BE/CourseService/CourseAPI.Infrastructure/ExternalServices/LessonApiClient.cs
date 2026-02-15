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
using TechTrioCourses.Shared.Dtos.Lesson;
using TechTrioCourses.Shared.Dtos.User;
using TechTrioCourses.Shared.Enums;

namespace CourseAPI.Infrastructure.ExternalServices
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

        public async Task<List<LessonResponse>> GetLessonsByCourseIdAsync(Guid courseId)
        {
            var cacheKey = $"CourseLessons_{courseId}";

            if (_cache.TryGetValue(cacheKey, out List<LessonResponse>? cached))
            {
                return cached!;
            }

            try
            {
                var lessons = await _httpClient
                    .GetFromJsonAsync<List<LessonResponse>>(
                        $"api/lessons/course/{courseId}")
                    ?? new();

                _cache.Set(cacheKey, lessons, TimeSpan.FromMinutes(30));
                return lessons;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex,
                    "Failed to fetch lessons for course {CourseId}", courseId);
                return new();
            }
        }

        public async Task<Dictionary<Guid, int>> GetAllLessonCountsAsync()
        {
            const string cacheKey = "AllLessonCounts";

            if (_cache.TryGetValue(cacheKey, out Dictionary<Guid, int>? cached))
            {
                _logger.LogInformation("Using cached lesson counts");
                return cached!;
            }

            try
            {
                var response = await _httpClient.GetAsync("api/Lessons");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to fetch Lessons. Status: {Status}",
                        response.StatusCode);
                    return new();
                }

                var Lessons = await response.Content
                    .ReadFromJsonAsync<List<LessonResponse>>() ?? new();

                var result = Lessons
                    .Where(q => q.Status == PublishStatusEnum.Published)
                    .GroupBy(q => q.CourseId)
                    .ToDictionary(g => g.Key, g => g.Count());

                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(30));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lesson counts");
                return new();
            }
        }


        public async Task PopulateLessonCountsAsync(List<CourseResponse> courses)
        {
            var lessonCounts = await GetAllLessonCountsAsync();

            foreach (var course in courses)
            {
                course.TotalLessons =
                    lessonCounts.TryGetValue(course.Id, out var count)
                        ? count
                        : 0;
            }
        }

    }
}
