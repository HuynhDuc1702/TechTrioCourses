using AutoMapper;
using System.Net.Http;
using UserAPI.DTOs.Request;
using UserAPI.DTOs.Response;
using UserAPI.Enums;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class UserCourseProgress : IUserCourseProgress
    {
        private readonly IUserCourseRepo _userCourseRepo;
        private readonly IUserLessonRepo _userLessonRepo;
        private readonly HttpClient _lessonAPIClient;
        private readonly ILogger<UserCourseProgress> _logger;
        private readonly IMapper _mapper;

        public UserCourseProgress(
            IUserCourseRepo userCourseRepo,
            IUserLessonRepo userLessonRepo,
            IHttpClientFactory httpClientFactory,
            ILogger<UserCourseProgress> logger,
            IMapper mapper)
        {
            _userCourseRepo = userCourseRepo;
            _userLessonRepo = userLessonRepo;
            _lessonAPIClient = httpClientFactory.CreateClient("LessonAPI");
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<object> RecaculateCourseProgress(Guid courseId, Guid userId)
        {
            _logger.LogInformation("🔍 [START] RecaculateCourseProgress called with UserId: {UserId}, CourseId: {CourseId}", userId, courseId);

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
            int completedLessons = userLessonsList.Count(ul => ul.Status == UserLessonStatus.Completed);

            // Fetch total lessons count for the course from LessonAPI
            try
            {
                var lessonsResponse = await _lessonAPIClient.GetAsync($"api/Lessons/course/{courseId}");
                if (lessonsResponse.IsSuccessStatusCode)
                {
                    var lessons = await lessonsResponse.Content.ReadFromJsonAsync<List<LessonResponse>>();
                    totalLessons = lessons?.Count ?? 0;
                }
                else
                {
                    _logger.LogWarning("Failed to fetch lessons from LessonAPI for CourseId: {CourseId}. Status: {StatusCode}",
  courseId, lessonsResponse.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch lessons for course {CourseId} from LessonAPI", courseId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching lessons for course {CourseId}", courseId);
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
