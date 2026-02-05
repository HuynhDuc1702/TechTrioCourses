using AutoMapper;

using TechTrioCourses.Shared.Enums;
using CourseAPI.Domain.Entities;


using Microsoft.Extensions.Caching.Memory;
using CourseAPI.Application.DTOs.Response;
using CourseAPI.Application.Interfaces;
using CourseAPI.Application.DTOs.Request;
using Microsoft.Extensions.Logging;
using CourseAPI.Application.Interfaces.IExternalServices;
using TechTrioCourses.Shared.Dtos.Category;

namespace CourseAPI.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _coursesRepo;
        private readonly IMapper _mapper;
        private readonly IUserApiClient _userAPIClient;
        private readonly ICategoryApiClient _categoryAPIClient;
        private readonly ILessonApiClient _lessonAPIClient;
        private readonly IQuizApiClient _quizAPIClient;
        private readonly ILogger<CourseService> _logger;
        private readonly IMemoryCache _cache;

        public CourseService(ICourseRepository coursesRepo, IMapper mapper,
            IHttpClientFactory httpClientFactory,
            ILogger<CourseService> logger,
            IMemoryCache cache,
             ICategoryApiClient categoryAPiClient,
             IQuizApiClient quizApiClient,
             IUserApiClient userApiClient,
            ILessonApiClient lessonApiClient)
        {
            _coursesRepo = coursesRepo;
            _mapper = mapper;
            _userAPIClient = userApiClient;
            _categoryAPIClient = categoryAPiClient;
            _lessonAPIClient = lessonApiClient;
            _quizAPIClient = quizApiClient;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IEnumerable<CourseResponse>> GetAllCoursesAsync()
        {
            var courses = await _coursesRepo.GetAllAsync();
            var result = _mapper.Map<List<CourseResponse>>(courses);

            await _categoryAPIClient.PopulateCategoryNameAsync(result);
            await _userAPIClient.PopulateCreatorNameAsync(result);
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


            await _quizAPIClient.PopulateQuizCountsAsync(courses);


            await _lessonAPIClient.PopulateLessonCountsAsync(courses);
        }







        public async Task<CourseResponse?> GetCourseByIdAsync(Guid id)
        {
            var course = await _coursesRepo.GetByIdAsync(id);
            if (course == null)
            {
                return null;
            }



            var result = _mapper.Map<CourseResponse>(course);


            if (course.CategoryId.HasValue)
            {
                var category = await _categoryAPIClient.GetCategoryByIdAsync(course.CategoryId.Value);
                result.CategoryName = category?.Name;
            }





            if (course.CreatorId.HasValue)
            {

                var creator = await _userAPIClient.GetUserByIdAsync(course.CreatorId.Value);
                result.CreatorName = creator?.FullName;

            }

            var lessons = await _lessonAPIClient.GetLessonsByCourseIdAsync(course.Id);

            result.TotalLessons = lessons?.Count(l => l.CourseId == result.Id
            && l.Status == PublishStatusEnum.Published) ?? 0;

            var quizzes = await _quizAPIClient.GetQuizzesByCourseIdAsync(course.Id);

            result.TotalQuizzes = quizzes?.Count(q => q.CourseId == result.Id
            && q.Status == PublishStatusEnum.Published) ?? 0;

            result.AverageRating = 0;

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
                return true;
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
                return true;
            }

            existingCourse.Status = PublishStatusEnum.Archived;
            existingCourse.UpdatedAt = DateTime.UtcNow;
            var updatedCourse = await _coursesRepo.UpdateAsync(existingCourse);

            return updatedCourse != null;
        }
    }
}
