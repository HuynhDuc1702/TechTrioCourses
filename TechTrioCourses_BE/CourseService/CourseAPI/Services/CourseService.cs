using AutoMapper;
using Azure;
using CourseAPI.DTOs.Request;
using CourseAPI.DTOs.Response;
using TechTrioCourses.Shared.Enums;
using CourseAPI.Models;
using CourseAPI.Repositories.Interfaces;
using CourseAPI.Services.Interfaces;
using Microsoft.VisualBasic;
using static CourseAPI.DTOs.Response.CourseResponse;
using Microsoft.Extensions.Caching.Memory;

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
        private readonly IMemoryCache _cache;

        public CourseService(ICourseRepo coursesRepo, IMapper mapper, IHttpClientFactory httpClientFactory, ILogger<CourseService> logger, IMemoryCache cache)
        {
            _coursesRepo = coursesRepo;
            _mapper = mapper;
            _userAPIClient = httpClientFactory.CreateClient("UserAPI");
            _categoryAPIClient = httpClientFactory.CreateClient("CategoryAPI");
            _lessonAPIClient = httpClientFactory.CreateClient("LessonAPI");
            _quizAPIClient = httpClientFactory.CreateClient("QuizAPI");
            _logger = logger;
            _cache = cache;
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
            string cacheKey = "AllLessonCounts";

            // Try to get from cache
            if (_cache.TryGetValue(cacheKey, out Dictionary<Guid, int> cachedLessonCounts))
            {
                _logger.LogInformation("? Using cached lesson counts");
                foreach (var course in courses)
                {
                    course.TotalLessons = cachedLessonCounts.TryGetValue(course.Id, out var count) ? count : 0;
                }
                return;
            }

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
                    && l.Status == PublishStatusEnum.Published)
                   .GroupBy(l => l.CourseId)
                     .ToDictionary(g => g.Key, g => g.Count());

                        // Cache for 30 minutes
                        var cacheOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                        };
                        _cache.Set(cacheKey, lessonCounts, cacheOptions);

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
            string cacheKey = "AllQuizCounts";

            // Try to get from cache
            if (_cache.TryGetValue(cacheKey, out Dictionary<Guid, int> cachedQuizCounts))
            {
                _logger.LogInformation("? Using cached quiz counts");
                foreach (var course in courses)
                {
                    course.TotalQuizzes = cachedQuizCounts.TryGetValue(course.Id, out var count) ? count : 0;
                }
                return;
            }

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
                                && q.Status == PublishStatusEnum.Published)
                             .GroupBy(q => q.CourseId)
                      .ToDictionary(g => g.Key, g => g.Count());

                        // Cache for 30 minutes
                        var cacheOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                        };
                        _cache.Set(cacheKey, quizCounts, cacheOptions);

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

            string cacheKey = "AllCategories";
            Dictionary<Guid, string>? categoryMap = null;

            // Try to get from cache first
            if (_cache.TryGetValue(cacheKey, out Dictionary<Guid, string>? cachedCategories))
            {
                _logger.LogInformation("? Using cached categories");
                categoryMap = cachedCategories;
            }
            else
            {
                try
                {
                    var response = await _categoryAPIClient.PostAsJsonAsync("api/categories/get-by-ids", categoryIds);

                    if (response.IsSuccessStatusCode)
                    {
                        var categories = await response.Content.ReadFromJsonAsync<List<CategoryResponse>>();
                        if (categories != null)
                        {
                            categoryMap = categories.ToDictionary(c => c.Id, c => c.Name);

                            // Cache for 30 minutes
                            var cacheOptions = new MemoryCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                            };
                            _cache.Set(cacheKey, categoryMap, cacheOptions);
                            _logger.LogInformation("? Cached categories");
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

            // Populate category names
            if (categoryMap != null)
            {
                foreach (var course in courses)
                {
                    if (course.CategoryId.HasValue && categoryMap.TryGetValue(course.CategoryId.Value, out var categoryName))
                    {
                        course.CategoryName = categoryName;
                    }
                }
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

            string cacheKey = "AllCreators";
            Dictionary<Guid, string>? userMap = null;

            // Try to get from cache first
            if (_cache.TryGetValue(cacheKey, out Dictionary<Guid, string>? cachedUsers))
            {
                _logger.LogInformation("? Using cached creators");
                userMap = cachedUsers;
            }
            else
            {
                try
                {
                    var response = await _userAPIClient.PostAsJsonAsync("api/Users/get-by-ids", creatorIds);

                    if (response.IsSuccessStatusCode)
                    {
                        var users = await response.Content.ReadFromJsonAsync<List<UserResponse>>();
                        if (users != null)
                        {
                            userMap = users.ToDictionary(c => c.Id, c => c.FullName);

                            // Cache for 30 minutes
                            var cacheOptions = new MemoryCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                            };
                            _cache.Set(cacheKey, userMap, cacheOptions);
                            _logger.LogInformation("? Cached creators");
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

            // Populate creator names
            if (userMap != null)
            {
                foreach (var course in courses)
                {
                    if (course.CreatorId.HasValue && userMap.TryGetValue(course.CreatorId.Value, out var creatorName))
                    {
                        course.CreatorName = creatorName;
                    }
                }
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

            // Fetch Category with caching
            if (course.CategoryId.HasValue)
            {
                string categoryCacheKey = $"Category_{course.CategoryId.Value}";

                if (!_cache.TryGetValue(categoryCacheKey, out CategoryResponse? category))
                {
                    try
                    {
                        category = await _categoryAPIClient.GetFromJsonAsync<CategoryResponse>($"api/categories/{course.CategoryId}");

                        if (category != null)
                        {
                            var cacheOptions = new MemoryCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                            };
                            _cache.Set(categoryCacheKey, category, cacheOptions);
                            _logger.LogInformation("? Cached category {CategoryId}", course.CategoryId);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        _logger.LogError(ex, "Failed to fetch category with ID {CategoryId} from CategoryAPI", course.CategoryId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error while fetching category for course {CourseId}", course.Id);
                    }
                }
                else
                {
                    _logger.LogInformation("? Using cached category {CategoryId}", course.CategoryId);
                }

                categoryName = category?.Name;
            }

            // Fetch User / Creator with caching
            if (course.CreatorId.HasValue)
            {
                string userCacheKey = $"User_{course.CreatorId.Value}";

                if (!_cache.TryGetValue(userCacheKey, out UserResponse? user))
                {
                    try
                    {
                        user = await _userAPIClient.GetFromJsonAsync<UserResponse>($"api/Users/{course.CreatorId}");

                        if (user != null)
                        {
                            var cacheOptions = new MemoryCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                            };
                            _cache.Set(userCacheKey, user, cacheOptions);
                            _logger.LogInformation("? Cached user {UserId}", course.CreatorId);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        _logger.LogError(ex, "Failed to fetch user with ID {CreatorId} from UserAPI", course.CreatorId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error while fetching creator for course {CourseId}", course.Id);
                    }
                }
                else
                {
                    _logger.LogInformation("? Using cached user {UserId}", course.CreatorId);
                }

                creatorName = user?.FullName;
            }

            // Fetch Lessons Count with caching
            string lessonsCacheKey = $"CourseLessons_{course.Id}";

            if (!_cache.TryGetValue(lessonsCacheKey, out int cachedLessonCount))
            {
                try
                {
                    var lessonsResponse = await _lessonAPIClient.GetAsync($"api/Lessons/course/{course.Id}");
                    if (lessonsResponse.IsSuccessStatusCode)
                    {
                        var lessons = await lessonsResponse.Content.ReadFromJsonAsync<List<LessonResponse>>();
                        totalLessons = lessons?.Count(l => l.CourseId == course.Id
                    && l.Status == PublishStatusEnum.Published) ?? 0;

                        var cacheOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
                        };
                        _cache.Set(lessonsCacheKey, totalLessons, cacheOptions);
                        _logger.LogInformation("? Cached lesson count for course {CourseId}", course.Id);
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
            }
            else
            {
                totalLessons = cachedLessonCount;
                _logger.LogInformation("? Using cached lesson count for course {CourseId}", course.Id);
            }

            // Fetch Quizzes Count with caching
            string quizzesCacheKey = $"CourseQuizzes_{course.Id}";

            if (!_cache.TryGetValue(quizzesCacheKey, out int cachedQuizCount))
            {
                try
                {
                    var quizzesResponse = await _quizAPIClient.GetAsync("api/Quizzes");
                    if (quizzesResponse.IsSuccessStatusCode)
                    {
                        var quizzes = await quizzesResponse.Content.ReadFromJsonAsync<List<QuizResponse>>();
                        totalQuizzes = quizzes?
                         .Count(q => q.CourseId == course.Id
                          && q.Status == PublishStatusEnum.Published) ?? 0;

                        var cacheOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
                        };
                        _cache.Set(quizzesCacheKey, totalQuizzes, cacheOptions);
                        _logger.LogInformation("? Cached quiz count for course {CourseId}", course.Id);
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
            }
            else
            {
                totalQuizzes = cachedQuizCount;
                _logger.LogInformation("? Using cached quiz count for course {CourseId}", course.Id);
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

            if (existingCourse.Status == PublishStatusEnum.Hidden)
            {
                return true; // Already disabled, no need to update
            }

            existingCourse.Status = PublishStatusEnum.Hidden;
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

            if (existingCourse.Status == PublishStatusEnum.Archived)
            {
                return true; // Already archived, no need to update
            }

            existingCourse.Status = PublishStatusEnum.Archived;
            existingCourse.UpdatedAt = DateTime.UtcNow;
            var updatedCourse = await _coursesRepo.UpdateAsync(existingCourse);

            return updatedCourse != null;
        }
    }
}
