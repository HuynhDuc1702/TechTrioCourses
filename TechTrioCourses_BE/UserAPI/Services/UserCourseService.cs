using AutoMapper;
using UserAPI.DTOs.Request;
using UserAPI.DTOs.Response;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class UserCourseService : IUserCourseService
    {
        private readonly IUserCourseRepo _userCourseRepo;
        private readonly IUserLessonService _userLessonService;
        private readonly IMapper _mapper;
        private readonly HttpClient _lessonAPIClient;
        private readonly HttpClient _quizAPIClient;
        private readonly ILogger<UserCourseService> _logger;
        public UserCourseService(IUserCourseRepo userCourseRepo, IMapper mapper, IHttpClientFactory httpClientFactory, ILogger<UserCourseService> logger, IUserLessonService userLessonService)
        {
            _userCourseRepo = userCourseRepo;
            _mapper = mapper;
            _lessonAPIClient = httpClientFactory.CreateClient("LessonAPI");
            _quizAPIClient = httpClientFactory.CreateClient("QuizAPI");
            _logger = logger;
            _userLessonService = userLessonService;
        }

        public async Task<UserCourseResponse?> GetUserCourseByIdAsync(Guid id)
        {
            var userCourse = await _userCourseRepo.GetByIdAsync(id);
            return userCourse == null ? null : _mapper.Map<UserCourseResponse>(userCourse);
        }

        public async Task<IEnumerable<UserCourseResponse>> GetAllUserCoursesAsync()
        {
            var userCourses = await _userCourseRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserCourseResponse>>(userCourses);
        }

        public async Task<IEnumerable<UserCourseResponse>> GetUserCoursesByUserIdAsync(Guid userId)
        {
            var userCourses = await _userCourseRepo.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<UserCourseResponse>>(userCourses);
        }

        public async Task<IEnumerable<UserCourseResponse>> GetUserCoursesByCourseIdAsync(Guid courseId)
        {
            var userCourses = await _userCourseRepo.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<UserCourseResponse>>(userCourses);
        }

        public async Task<UserCourseResponse?> GetUserCourseByUserAndCourseAsync(Guid userId, Guid courseId)
        {
            var userCourse = await _userCourseRepo.GetByUserAndCourseAsync(userId, courseId);
            return userCourse == null ? null : _mapper.Map<UserCourseResponse>(userCourse);
        }

        public async Task<UserCourseResponse?> CreateUserCourseAsync(CreateUserCourseRequest request)
        {
            // Check if user course already exists
            if (await _userCourseRepo.UserCourseExistsAsync(request.UserId, request.CourseId))
            {
                return null;
            }

            // Create user course
            var userCourse = _mapper.Map<UserCourse>(request);
            var createdUserCourse = await _userCourseRepo.CreateUserCourseAsync(userCourse);

            return _mapper.Map<UserCourseResponse>(createdUserCourse);
        }

        public async Task<UserCourseResponse?> UpdateUserCourseAsync(Guid id)
        {
            var userCourse = await _userCourseRepo.GetByIdAsync(id);
            if (userCourse == null)
            {
                return null;
            }

            // Get user lessons for this course to calculate progress
            var userLessons = await _userLessonService.GetUserLessonsByUserAndCourseAsync(userCourse.UserId, userCourse.CourseId);
            var userLessonsList = userLessons.ToList();

            int totalLessons = 0;
            int completedLessons = userLessonsList.Count(ul => ul.Status == Enums.UserLessonStatus.Completed);

            // Fetch total lessons count for the course from LessonAPI
            try
            {
                var lessonsResponse = await _lessonAPIClient.GetAsync($"api/Lessons/by-course/{userCourse.CourseId}");
                if (lessonsResponse.IsSuccessStatusCode)
                {
                    var lessons = await lessonsResponse.Content.ReadFromJsonAsync<List<LessonResponse>>();
                    totalLessons = lessons?.Count ?? 0;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch lessons for course {CourseId} from LessonAPI", userCourse.CourseId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching lessons for course {CourseId}", userCourse.CourseId);
            }

            // Calculate progress
            if (totalLessons > 0)
            {
                userCourse.Progress = (double)completedLessons / totalLessons * 100;
            }
            else
            {
                userCourse.Progress = 0;
            }

            // Update status based on progress
            if (userCourse.Progress >= 100)
            {
                userCourse.Status = Enums.UserCourseStatus.Completed;
                if (userCourse.CompletedAt == null)
                {
                    userCourse.CompletedAt = DateTime.UtcNow;
                }
            }
            else if (userCourse.Progress > 0)
            {
                userCourse.Status = Enums.UserCourseStatus.In_progress;
            }

            await _userCourseRepo.UpdateUserCourseAsync(userCourse);

            return _mapper.Map<UserCourseResponse>(userCourse);
        }

        public async Task<bool> DeleteUserCourseAsync(Guid id)
        {
            return await _userCourseRepo.DeleteUserCourseAsync(id);
        }
    }
}
