using AutoMapper;
using Azure;
using CourseAPI.DTOs.Request;
using CourseAPI.DTOs.Response;
using CourseAPI.Enums;
using CourseAPI.Models;
using CourseAPI.Repositories.Interfaces;
using CourseAPI.Services.Interfaces;
using Microsoft.VisualBasic;
using static CourseAPI.DTOs.Response.CourseResponse;

namespace CourseAPI.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepo _coursesRepo;
        private readonly IMapper _mapper;
        private readonly HttpClient _userAPIClient;
        private readonly HttpClient _categoryAPIClient;
        private readonly HttpClient _lessonAPIClient;
        private readonly HttpClient _quizAPIClient;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICourseRepo coursesRepo, IMapper mapper, IHttpClientFactory httpClientFactory, ILogger<CourseService> logger)
        {
            _coursesRepo = coursesRepo;
            _mapper = mapper;
            _userAPIClient = httpClientFactory.CreateClient("UserAPI");
            _categoryAPIClient = httpClientFactory.CreateClient("CategoryAPI");
            _lessonAPIClient = httpClientFactory.CreateClient("LessonAPI");
            _quizAPIClient = httpClientFactory.CreateClient("QuizAPI");
            _logger = logger;
        }

        public async Task<IEnumerable<CourseResponse>> GetAllCoursesAsync()
        {
            var courses = await _coursesRepo.GetAllAsync();
            var result = _mapper.Map<List<CourseResponse>>(courses);

            await PopulateCategoryNameAsync(result);
            await PopulateCreatorNameAsync(result);
            await PopulateLessonAndQuizCountsAsync(result);

            return result;
        }

        private async Task PopulateLessonAndQuizCountsAsync(List<CourseResponse> courses)
        {
            var courseIds = courses.Select(c => c.Id).Distinct().ToList();

            if (!courseIds.Any())
            {
                return;
            }

            // Fetch lesson counts for all courses
            await PopulateLessonCountsAsync(courses, courseIds);

            // Fetch quiz counts for all courses
            await PopulateQuizCountsAsync(courses, courseIds);
        }

        private async Task PopulateLessonCountsAsync(List<CourseResponse> courses, List<Guid> courseIds)
        {
            try
            {
                // Fetch all lessons
                var response = await _lessonAPIClient.GetAsync("api/Lessons");

                if (response.IsSuccessStatusCode)
                {
                    var lessons = await response.Content.ReadFromJsonAsync<List<LessonResponse>>();
                    if (lessons != null)
                    {
                        // Group lessons by CourseId and count them
                        var lessonCounts = lessons
                    .Where(l => courseIds.Contains(l.CourseId)
                    && l.Status == LessonStatusEnum.Published)
                   .GroupBy(l => l.CourseId)
                     .ToDictionary(g => g.Key, g => g.Count());

                        // Populate the TotalLessons for each course
                        foreach (var course in courses)
                        {
                            course.TotalLessons = lessonCounts.TryGetValue(course.Id, out var count) ? count : 0;
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to fetch lessons. Status: {StatusCode}, Reason: {Reason}",
                               response.StatusCode,
                   response.ReasonPhrase);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while fetching lesson data from LessonAPI");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while populating lesson counts");
            }
        }

        private async Task PopulateQuizCountsAsync(List<CourseResponse> courses, List<Guid> courseIds)
        {
            try
            {
                // Fetch all quizzes
                var response = await _quizAPIClient.GetAsync("api/Quizzes");

                if (response.IsSuccessStatusCode)
                {
                    var quizzes = await response.Content.ReadFromJsonAsync<List<QuizResponse>>();
                    if (quizzes != null)
                    {
                        // Group quizzes by CourseId and count them
                        var quizCounts = quizzes
                         .Where(q => courseIds.Contains(q.CourseId)
                                && q.Status == QuizzStatusEnum.Published)
                             .GroupBy(q => q.CourseId)
                      .ToDictionary(g => g.Key, g => g.Count());

                        // Populate the TotalQuizzes for each course
                        foreach (var course in courses)
                        {
                            course.TotalQuizzes = quizCounts.TryGetValue(course.Id, out var count) ? count : 0;
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to fetch quizzes. Status: {StatusCode}, Reason: {Reason}",
                  response.StatusCode,
                  response.ReasonPhrase);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while fetching quiz data from QuizAPI");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while populating quiz counts");
            }
        }

        public async Task PopulateCategoryNameAsync(List<CourseResponse> courses)
        {
            var categoryIds = courses
     .Where(c => c.CategoryId.HasValue)
     .Select(c => c.CategoryId!.Value)
         .Distinct()
           .ToList();

            if (!categoryIds.Any())
            {
                return;
            }
            try
            {
                var response = await _categoryAPIClient.PostAsJsonAsync("api/categories/get-by-ids", categoryIds);

                if (response.IsSuccessStatusCode)
                {
                    var categories = await response.Content.ReadFromJsonAsync<List<CategoryResponse>>();
                    if (categories != null)
                    {
                        var categoryMap = categories.ToDictionary(c => c.Id, c => c.Name);

                        foreach (var course in courses)
                        {
                            if (course.CategoryId.HasValue && categoryMap.TryGetValue(course.CategoryId.Value, out var categoryName))
                            {
                                course.CategoryName = categoryName;
                            }
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to fetch categories. Status: {StatusCode}, Reason: {Reason}",
           response.StatusCode,
                 response.ReasonPhrase);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while fetching category data from CategoryAPI");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while populating category names");
            }
        }

        public async Task PopulateCreatorNameAsync(List<CourseResponse> courses)
        {
            var creatorIds = courses
                .Where(c => c.CreatorId.HasValue)
                     .Select(c => c.CreatorId!.Value)
                  .Distinct()
                 .ToList();

            if (!creatorIds.Any())
            {
                return;
            }
            try
            {
                var response = await _userAPIClient.PostAsJsonAsync("api/Users/get-by-ids", creatorIds);

                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<List<UserResponse>>();
                    if (users != null)
                    {
                        var userMap = users.ToDictionary(c => c.Id, c => c.FullName);

                        foreach (var course in courses)
                        {
                            if (course.CreatorId.HasValue && userMap.TryGetValue(course.CreatorId.Value, out var creatorName))
                            {
                                course.CreatorName = creatorName;
                            }
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to fetch users. Status: {StatusCode}, Reason: {Reason}",
                  response.StatusCode,
            response.ReasonPhrase);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while fetching users data from UserAPI");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while populating creator names");
            }
        }

        public async Task<CourseResponse?> GetCourseByIdAsync(Guid id)
        {
            var course = await _coursesRepo.GetByIdAsync(id);

            if (course == null)
            {
                return null;
            }

            string? categoryName = null;
            string? creatorName = null;
            int totalLessons = 0;
            int totalQuizzes = 0;

            // Fetch Category
            try
            {
                var category = await _categoryAPIClient.GetFromJsonAsync<CategoryResponse>($"api/categories/{course.CategoryId}");
                categoryName = category?.Name;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch category with ID {CategoryId} from CategoryAPI", course.CategoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching category for course {CourseId}", course.Id);
            }

            // Fetch User / Creator
            try
            {
                var user = await _userAPIClient.GetFromJsonAsync<UserResponse>($"api/Users/{course.CreatorId}");
                creatorName = user?.FullName;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch user with ID {CreatorId} from UserAPI", course.CreatorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching creator for course {CourseId}", course.Id);
            }

            // Fetch Lessons Count
            try
            {
                var lessonsResponse = await _lessonAPIClient.GetAsync($"api/Lessons/course/{course.Id}");
                if (lessonsResponse.IsSuccessStatusCode)
                {
                    var lessons = await lessonsResponse.Content.ReadFromJsonAsync<List<LessonResponse>>();
                    totalLessons = lessons?.Count(l => l.CourseId == course.Id
                                                         && l.Status == LessonStatusEnum.Published) ?? 0;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch lessons for course {CourseId} from LessonAPI", course.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching lessons for course {CourseId}", course.Id);
            }

            // Fetch Quizzes Count
            try
            {
                var quizzesResponse = await _quizAPIClient.GetAsync("api/Quizzes");
                if (quizzesResponse.IsSuccessStatusCode)
                {
                    var quizzes = await quizzesResponse.Content.ReadFromJsonAsync<List<QuizResponse>>();
                    totalQuizzes = quizzes?

                        .Count(q => q.CourseId == course.Id
                                    && q.Status == QuizzStatusEnum.Published) ?? 0;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch quizzes for course {CourseId} from QuizAPI", course.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching quizzes for course {CourseId}", course.Id);
            }

            var result = _mapper.Map<CourseResponse>(course);
            result.CategoryName = categoryName;
            result.CreatorName = creatorName;
            result.TotalLessons = totalLessons;
            result.TotalQuizzes = totalQuizzes;
            result.AverageRating = 0; // Set to 0 for now (not implemented)

            return result;
        }

        public async Task<CourseResponse> CreateCourseAsync(CreateCourseRequest request)
        {
            var course = _mapper.Map<Course>(request);

            var createdCourse = await _coursesRepo.CreateAsync(course);

            return _mapper.Map<CourseResponse>(createdCourse);
        }

        public async Task<CourseResponse?> UpdateCourseAsync(Guid id, UpdateCourseRequest request)
        {
            var existingCourse = await _coursesRepo.GetByIdAsync(id);

            if (existingCourse == null)
            {
                return null;
            }

            // Map only non-null properties from request to existing course
            if (request.Title != null)
                existingCourse.Title = request.Title;

            if (request.Description != null)
                existingCourse.Description = request.Description;

            if (request.CategoryId.HasValue)
                existingCourse.CategoryId = request.CategoryId;

            if (request.Status.HasValue)
                existingCourse.Status = request.Status.Value;

            var updatedCourse = await _coursesRepo.UpdateAsync(existingCourse);

            if (updatedCourse == null)
            {
                return null;
            }

            return _mapper.Map<CourseResponse>(updatedCourse);
        }

        public async Task<bool> DeleteCourseAsync(Guid id)
        {
            return await _coursesRepo.DeleteAsync(id);
        }

        public async Task<bool> DisableCourseAsync(Guid id)
        {
            var existingCourse = await _coursesRepo.GetByIdAsync(id);

            if (existingCourse == null)
            {
                return false;
            }

            if (existingCourse.Status == CourseStatusEnum.Hidden)
            {
                return true; // Already disabled, no need to update
            }

            existingCourse.Status = CourseStatusEnum.Hidden;
            existingCourse.UpdatedAt = DateTime.UtcNow;
            var updatedCourse = await _coursesRepo.UpdateAsync(existingCourse);

            return updatedCourse != null;
        }

        public async Task<bool> ArchiveCourseAsync(Guid id)
        {
            var existingCourse = await _coursesRepo.GetByIdAsync(id);

            if (existingCourse == null)
            {
                return false;
            }

            if (existingCourse.Status == CourseStatusEnum.Archived)
            {
                return true; // Already archived, no need to update
            }

            existingCourse.Status = CourseStatusEnum.Archived;
            existingCourse.UpdatedAt = DateTime.UtcNow;
            var updatedCourse = await _coursesRepo.UpdateAsync(existingCourse);

            return updatedCourse != null;
        }
    }
}
