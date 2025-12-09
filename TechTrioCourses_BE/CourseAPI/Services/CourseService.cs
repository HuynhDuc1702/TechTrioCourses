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
        private readonly ILogger<CourseService> _logger;
        public CourseService(ICourseRepo coursesRepo, IMapper mapper, IHttpClientFactory httpClientFactory, ILogger<CourseService> logger)
        {
            _coursesRepo = coursesRepo;
            _mapper = mapper;
            _userAPIClient = httpClientFactory.CreateClient("UserAPI");
            _categoryAPIClient = httpClientFactory.CreateClient("CategoryAPI");
            _logger = logger;
        }

        public async Task<IEnumerable<CourseResponse>> GetAllCoursesAsync()
        {
            var courses = await _coursesRepo.GetAllAsync();
            var result = _mapper.Map<List<CourseResponse>>(courses);

            await PopulateCategoryNameAsync(result);
            await PopulateCreatorNameAsync(result);

            return result;

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

            var result = _mapper.Map<CourseResponse>(course);
            result.CategoryName = categoryName;
            result.CreatorName = creatorName;

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
                return true; // Already disabled, no need to update
            }
            existingCourse.Status = CourseStatusEnum.Archived;
            existingCourse.UpdatedAt = DateTime.UtcNow;
            var updatedCourse = await _coursesRepo.UpdateAsync(existingCourse);

            return updatedCourse != null;

        }


    }
}
