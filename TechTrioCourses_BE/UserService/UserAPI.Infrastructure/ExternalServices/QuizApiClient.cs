using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using TechTrioCourses.Shared.Dtos.Quiz;
using TechTrioCourses.Shared.Enums;
using UserAPI.Application.Interfaces.IExternalServices;

namespace UserAPI.Infrastructure.ExternalServices
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

        public async Task<int> GetQuizCountByCourseIdAsync(Guid courseId)
        {
            var cacheKey = $"CourseQuizzies_{courseId}";


            if (_cache.TryGetValue(cacheKey, out int cachedCount))
            {
                return cachedCount;
            }

            try
            {
                var quizzes = await _httpClient
                            .GetFromJsonAsync<List<QuizResponse>>(
                             $"api/quizzes/course/{courseId}")
                             ?? new List<QuizResponse>();

                var count = quizzes.Count(q => q.Status == PublishStatusEnum.Published);


                _cache.Set(cacheKey, quizzes, TimeSpan.FromMinutes(30));

                return count;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(
                ex,
                "Failed to fetch quizzes for course {CourseId}",
                 courseId);

                return 0;
            }
        }


    }
}
