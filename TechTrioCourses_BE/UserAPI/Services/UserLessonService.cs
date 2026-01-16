using AutoMapper;
using TechTrioCourses.Shared.Enums;
using UserAPI.DTOs.Request.UserLesson;
using UserAPI.DTOs.Response.UserLesson;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace UserAPI.Services
{
    public class UserLessonService : IUserLessonService
    {
        private readonly IUserLessonRepo _userLessonRepo;
        private readonly IUserCourseProgress _userCourseProgress;
        private readonly IMapper _mapper;
        private readonly ILogger<UserLessonService> _logger;

        public UserLessonService(
            IUserLessonRepo userLessonRepo, 
            IMapper mapper, 
            IUserCourseProgress userCourseProgress,
            ILogger<UserLessonService> logger)
        {
            _userLessonRepo = userLessonRepo;
            _mapper = mapper;
            _userCourseProgress = userCourseProgress;
            _logger = logger;
        }

        public async Task<UserLessonResponse?> GetUserLessonByIdAsync(Guid id)
        {
            var userLesson = await _userLessonRepo.GetByIdAsync(id);
            return userLesson == null ? null : _mapper.Map<UserLessonResponse>(userLesson);
        }

        public async Task<IEnumerable<UserLessonResponse>> GetAllUserLessonsAsync()
        {
            var userLessons = await _userLessonRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserLessonResponse>>(userLessons);
        }

        public async Task<IEnumerable<UserLessonResponse>> GetUserLessonsByUserIdAsync(Guid userId)
        {
            var userLessons = await _userLessonRepo.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<UserLessonResponse>>(userLessons);
        }

        public async Task<IEnumerable<UserLessonResponse>> GetUserLessonsByLessonIdAsync(Guid lessonId)
        {
            var userLessons = await _userLessonRepo.GetByLessonIdAsync(lessonId);
            return _mapper.Map<IEnumerable<UserLessonResponse>>(userLessons);
        }

        public async Task<UserLessonResponse?> GetUserLessonByUserAndLessonAsync(Guid userId, Guid lessonId)
        {
            var userLesson = await _userLessonRepo.GetByUserAndLessonAsync(userId, lessonId);
            return userLesson == null ? null : _mapper.Map<UserLessonResponse>(userLesson);
        }

        public async Task<UserLessonResponse?> CreateUserLessonAsync(CreateUserLessonRequest request)
        {
            // Check if user lesson already exists
            var existingUserLesson = await _userLessonRepo.GetByUserAndLessonAsync(request.UserId, request.LessonId);

            if (existingUserLesson != null)
            {
                // Already exists, return existing
              return _mapper.Map<UserLessonResponse>(existingUserLesson);
            }

            // Create new user lesson - automatically mark as completed
            var userLesson = _mapper.Map<UserLesson>(request);
            userLesson.Status = UserLessonStatusEnum.Completed;
            userLesson.CompletedAt = DateTime.UtcNow;

            var createdUserLesson = await _userLessonRepo.CreateUserLessonAsync(userLesson);

            // Trigger progress recalculation
            try
            {
                await _userCourseProgress.RecaculateCourseProgress(request.CourseId, request.UserId);
            }
            catch (Exception ex)
            {
       _logger.LogError(ex, "Error during course progress recalculation for CourseId: {CourseId}, UserId: {UserId}", 
         request.CourseId, request.UserId);
            }

            return _mapper.Map<UserLessonResponse>(createdUserLesson);
        }


        public async Task<bool> DeleteUserLessonAsync(Guid id)
        {
            return await _userLessonRepo.DeleteUserLessonAsync(id);
        }

        public async Task<IEnumerable<UserLessonResponse>> GetUserLessonsByCourseIdAsync(Guid courseId)
        {
            var userLessons = await _userLessonRepo.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<UserLessonResponse>>(userLessons);
        }

        public async Task<IEnumerable<UserLessonResponse>> GetUserLessonsByUserAndCourseAsync(Guid userId, Guid courseId)
        {
            var userLessons = await _userLessonRepo.GetByUserAndCourseAsync(userId, courseId);
            return _mapper.Map<IEnumerable<UserLessonResponse>>(userLessons);
        }

       
    }
}
