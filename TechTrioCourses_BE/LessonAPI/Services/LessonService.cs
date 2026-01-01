using AutoMapper;
using Azure;
using LessonAPI.Enums;
using LessonAPI.Models;
using LessonAPI.DTOs.Request;
using LessonAPI.DTOs.Response;
using LessonAPI.Repositories.Interfaces;
using LessonAPI.Services.Interfaces;
using Microsoft.VisualBasic;
using static LessonAPI.DTOs.Response.LessonResponse;

namespace LessonAPI.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepo _lessonsRepo;
        private readonly IMapper _mapper;
        private readonly HttpClient _courseAPIClient;

        private readonly ILogger<LessonService> _logger;
        public LessonService(ILessonRepo lessonsRepo, IMapper mapper, IHttpClientFactory httpClientFactory, ILogger<LessonService> logger)
        {
            _lessonsRepo = lessonsRepo;
            _mapper = mapper;
            _courseAPIClient = httpClientFactory.CreateClient("CourseAPI");

            _logger = logger;
        }

        public async Task<IEnumerable<LessonResponse>> GetAllLessonsAsync()
        {
            var lessons = await _lessonsRepo.GetAllAsync();
            var result = _mapper.Map<List<LessonResponse>>(lessons);


            return result;

        }
        public async Task<IEnumerable<LessonResponse>> GetAllLessonsByCourseAsync(Guid courseId)
        {
            var lessons = await _lessonsRepo.GetAllLessonByCourseAsync(courseId);
            var result = _mapper.Map<List<LessonResponse>>(lessons);


            return result;

        }

        public async Task<LessonResponse?> GetLessonByIdAsync(Guid id)
        {
            var lesson = await _lessonsRepo.GetByIdAsync(id);

            if (lesson == null)
            {
                return null;
            }

            string? courseTitle = null;



            try
            {
                var course = await _courseAPIClient.GetFromJsonAsync<CourseResponse>($"api/courses/{lesson.CourseId}");
                courseTitle = course?.Title;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch category with ID {CourseID} from CourseAPI", lesson.CourseId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching course Title for lesson {LessonId}", lesson.Id);
            }



            var result = _mapper.Map<LessonResponse>(lesson);
            result.CourseName = courseTitle;


            return result;
        }


        public async Task<LessonResponse> CreateLessonAsync(CreateLessonRequest request)
        {
            var lesson = _mapper.Map<Lesson>(request);

            var createdLesson = await _lessonsRepo.CreateAsync(lesson);

            return _mapper.Map<LessonResponse>(createdLesson);
        }

        public async Task<LessonResponse?> UpdateLessonAsync(Guid id, UpdateLessonRequest request)
        {
            var existingLesson = await _lessonsRepo.GetByIdAsync(id);

            if (existingLesson == null)
            {
                return null;
            }

            // Map only non-null properties from request to existing lesson
            if (request.Title != null)
                existingLesson.Title = request.Title;

            if (request.Content != null)
                existingLesson.Content = request.Content;

            if (request.MediaUrl != null)
                existingLesson.MediaUrl = request.MediaUrl;

            if (request.MediaType.HasValue)
                existingLesson.MediaType = request.MediaType.Value;

            if (request.OrderIndex.HasValue)
                existingLesson.OrderIndex = request.OrderIndex.Value;

            if (request.Status.HasValue)
                existingLesson.Status = request.Status.Value;

            existingLesson.UpdatedAt = DateTime.UtcNow;

            var updatedLesson = await _lessonsRepo.UpdateAsync(existingLesson);

            if (updatedLesson == null)
            {
                return null;
            }

            return _mapper.Map<LessonResponse>(updatedLesson);
        }

        public async Task<bool> DeleteLessonAsync(Guid id)
        {
            return await _lessonsRepo.DeleteAsync(id);
        }
        public async Task<bool> DisableLessonAsync(Guid id)
        {
            var existingLesson = await _lessonsRepo.GetByIdAsync(id);

            if (existingLesson == null)
            {
                return false;

            }
            if (existingLesson.Status == LessonStatusEnum.Hidden)
            {
                return true; // Already disabled, no need to update
            }
            existingLesson.Status = LessonStatusEnum.Hidden;
            existingLesson.UpdatedAt = DateTime.UtcNow;
            var updatedLesson = await _lessonsRepo.UpdateAsync(existingLesson);

            return updatedLesson != null;

        }
        public async Task<bool> ArchiveLessonAsync(Guid id)
        {
            var existingLesson = await _lessonsRepo.GetByIdAsync(id);

            if (existingLesson == null)
            {
                return false;

            }
            if (existingLesson.Status == LessonStatusEnum.Archived)
            {
                return true; // Already disabled, no need to update
            }
            existingLesson.Status = LessonStatusEnum.Archived;
            existingLesson.UpdatedAt = DateTime.UtcNow;
            var updatedLesson = await _lessonsRepo.UpdateAsync(existingLesson);

            return updatedLesson != null;

        }


    }
}
