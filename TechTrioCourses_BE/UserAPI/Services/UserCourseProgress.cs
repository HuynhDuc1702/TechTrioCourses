using AutoMapper;
using System.Net.Http;
using UserAPI.DTOs.Response.UserCourse;
using TechTrioCourses.Shared.Enums;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace UserAPI.Services
{
    public class UserCourseProgress : IUserCourseProgress
    {
        private readonly IUserCourseRepo _userCourseRepo;
        private readonly IUserLessonRepo _userLessonRepo;
        private readonly HttpClient _lessonAPIClient;
        private readonly ILogger<UserCourseProgress> _logger;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public UserCourseProgress(
            IUserCourseRepo userCourseRepo,
            IUserLessonRepo userLessonRepo,
            IHttpClientFactory httpClientFactory,
            ILogger<UserCourseProgress> logger,
            IMapper mapper,
            IMemoryCache cache)
        {
            _userCourseRepo = userCourseRepo;
            _userLessonRepo = userLessonRepo;
            _lessonAPIClient = httpClientFactory.CreateClient("LessonAPI");
            _logger = logger;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<object> RecaculateCourseProgress(Guid courseId, Guid userId)
        {
            _logger.LogInformation("?? [START] RecaculateCourseProgress called with UserId: {UserId}, CourseId: {CourseId}", userId, courseId);

            // Get the UserCourse by ID
            var userCourse = await _userCourseRepo.GetByUserAndCourseAsync(userId, courseId);

            if (userCourse == null)
            {
                _logger.LogWarning("UserCourse not found for UserId: {UserId}, CourseId: {CourseId}", userId, courseId);
                return null;
            }

            // Get user lessons for this course to calculate progress
            var userLessons = await _userLessonRepo.GetByUserAndCourseAsync(userId, courseId);
            var userLessonsList = userLessons.ToList();

            int totalLessons = 0;
            int completedLessons = userLessonsList.Count(ul => ul.Status == UserLessonStatusEnum.Completed);

            // Try to get lesson count from cache
            string cacheKey = $"LessonCount_{courseId}";

            if (_cache.TryGetValue(cacheKey, out int cachedCount))
            {
                totalLessons = cachedCount;
                _logger.LogInformation("? Using cached lesson count for CourseId: {CourseId}, Count: {Count}", courseId, cachedCount);
            }
            else
            {
                // Cache miss - Fetch from LessonAPI
                _logger.LogInformation("?? Cache miss. Fetching lessons from LessonAPI for CourseId: {CourseId}", courseId);

                try
                {
                    var lessonsResponse = await _lessonAPIClient.GetAsync($"api/Lessons/course/{courseId}");
                    if (lessonsResponse.IsSuccessStatusCode)
                    {
                        var lessons = await lessonsResponse.Content.ReadFromJsonAsync<List<LessonResponse>>();
                        totalLessons = lessons?.Count ?? 0;

                        // Cache the result for 1 hour
                        var cacheOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                        };
                        _cache.Set(cacheKey, totalLessons, cacheOptions);
                        _logger.LogInformation("?? Cached lesson count for CourseId: {CourseId}, Count: {Count}", courseId, totalLessons);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to fetch lessons from LessonAPI for CourseId: {CourseId}. Status: {StatusCode}. Using fallback.",
                            courseId, lessonsResponse.StatusCode);
                        // FALLBACK: Use count of user's existing lessons (minimum known)
                        totalLessons = userLessonsList.Count;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to fetch lessons for course {CourseId} from LessonAPI. Using fallback.", courseId);
                    // FALLBACK: Use count of user's existing lessons (minimum known)
                    totalLessons = userLessonsList.Count;
                }
            }

            // Calculate progress and round to 2 decimal places
            if (totalLessons > 0)
            {
                double rawProgress = (double)completedLessons / totalLessons * 100;
                userCourse.Progress = Math.Ceiling(rawProgress);
            }
            else
            {
                userCourse.Progress = 0;
            }

            await _userCourseRepo.UpdateUserCourseAsync(userCourse);

            _logger.LogInformation("Course progress recalculated for UserId: {UserId}, CourseId: {CourseId}, Progress: {Progress}%",
   userId, courseId, userCourse.Progress);

            return _mapper.Map<UserCourseResponse>(userCourse);
        }
    }

    // DTO class for LessonAPI response
    public class LessonResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
    }
}
