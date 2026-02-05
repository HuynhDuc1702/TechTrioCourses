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
using TechTrioCourses.Shared.Dtos.Quiz;
using TechTrioCourses.Shared.Dtos.User;
using TechTrioCourses.Shared.Enums;

namespace CourseAPI.Infrastructure.ExternalServices
{
    public class QuizApiClient : IQuizApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<QuizApiClient> _logger;
        private readonly IMemoryCache _cache;

        public QuizApiClient(
            IHttpClientFactory httpClientFactory,
            ILogger<QuizApiClient> logger,
            IMemoryCache cache)
        {
            _httpClient = httpClientFactory.CreateClient("QuizAPI");
            _logger = logger;
            _cache = cache;
        }

        public async Task<List<QuizResponse>> GetQuizzesByCourseIdAsync(Guid courseId)
        {
            var cacheKey = $"CourseQuizzes_{courseId}";

            if (_cache.TryGetValue(cacheKey, out List<QuizResponse>? cached))
            {
                return cached!;
            }

            try
            {
                var quizzes = await _httpClient
                    .GetFromJsonAsync<List<QuizResponse>>(
                        $"api/quizzes/course/{courseId}")
                    ?? new();

                _cache.Set(cacheKey, quizzes, TimeSpan.FromMinutes(30));
                return quizzes;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex,
                    "Failed to fetch quizzes for course {CourseId}", courseId);
                return new();
            }
        }

        public async Task<Dictionary<Guid, int>> GetAllQuizCountsAsync()
        {
            const string cacheKey = "AllQuizCounts";

            if (_cache.TryGetValue(cacheKey, out Dictionary<Guid, int>? cached))
            {
                _logger.LogInformation("Using cached quiz counts");
                return cached!;
            }

            try
            {
                var response = await _httpClient.GetAsync("api/Quizzes");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to fetch quizzes. Status: {Status}",
                        response.StatusCode);
                    return new();
                }

                var quizzes = await response.Content
                    .ReadFromJsonAsync<List<QuizResponse>>() ?? new();

                var result = quizzes
                    .Where(q => q.Status == PublishStatusEnum.Published)
                    .GroupBy(q => q.CourseId)
                    .ToDictionary(g => g.Key, g => g.Count());

                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(30));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching quiz counts");
                return new();
            }
        }


        public async Task PopulateQuizCountsAsync(List<CourseResponse> courses)
        {
            var quizCounts = await GetAllQuizCountsAsync();

            foreach (var course in courses)
            {
                course.TotalQuizzes =
                    quizCounts.TryGetValue(course.Id, out var count)
                        ? count
                        : 0;
            }
        }

    }
}
