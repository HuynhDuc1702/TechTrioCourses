using AutoMapper;
using Microsoft.VisualBasic;

using TechTrioCourses.Shared.Enums;
using LessonAPI.Application.DTOs.Request;
using LessonAPI.Application.DTOs.Response;
using LessonAPI.Application.Interfaces;
using Microsoft.Extensions.Logging;
using LessonAPI.Domain.Entities;
using LessonAPI.Application.Interfaces.IExternalServices;

namespace LessonAPI.Application.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonsRepo;
        private readonly IMapper _mapper;
        private readonly ICourseApiClient _courseAPIClient;

        private readonly ILogger<LessonService> _logger;
        public LessonService(ILessonRepository lessonsRepo,
            IMapper mapper,
            IHttpClientFactory httpClientFactory,
            ILogger<LessonService> logger,
            ICourseApiClient courseApiClient)
        {
            _lessonsRepo = lessonsRepo;
            _mapper = mapper;
            _courseAPIClient = courseApiClient;

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




            var result = _mapper.Map<LessonResponse>(lesson);

            var course = await _courseAPIClient.GetCourseByIdAsync(lesson.CourseId);
            result.CourseName = course?.Title;


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
            if (existingLesson.Status == PublishStatusEnum.Hidden)
            {
                return true;
            }
            existingLesson.Status = PublishStatusEnum.Hidden;
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
            if (existingLesson.Status == PublishStatusEnum.Archived)
            {
                return true;
            }
            existingLesson.Status = PublishStatusEnum.Archived;
            existingLesson.UpdatedAt = DateTime.UtcNow;
            var updatedLesson = await _lessonsRepo.UpdateAsync(existingLesson);

            return updatedLesson != null;

        }


    }
}
