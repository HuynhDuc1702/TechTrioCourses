using AutoMapper;
using TechTrioCourses.Shared.Enums;
using UserAPI.Domain.Entities;
using Microsoft.Extensions.Logging;
using UserAPI.Application.Interfaces.IServices;
using UserAPI.Application.DTOs.Response.UserLesson;
using UserAPI.Application.DTOs.Request.UserLesson;
using UserAPI.Application.Interfaces.IRepositories;

namespace UserAPI.Application.Services
{
    public class UserLessonService : IUserLessonService
    {
        private readonly IUserLessonRepository _userLessonRepo;
        private readonly IUserCourseProgressService _userCourseProgress;
        private readonly IMapper _mapper;
        private readonly ILogger<UserLessonService> _logger;

        public UserLessonService(
            IUserLessonRepository userLessonRepo,
            IMapper mapper,
            IUserCourseProgressService userCourseProgress,
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

            if (await _userLessonRepo.ExistsAsync(request.UserId, request.LessonId))
            {
                throw new InvalidOperationException("User already completed this lesson.");
            }
            var userLesson = _mapper.Map<UserLesson>(request);
            userLesson.Status = UserLessonStatusEnum.Completed;
            userLesson.CompletedAt = DateTime.UtcNow;

            var createdUserLesson = await _userLessonRepo.CreateAsync(userLesson);

    
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
            return await _userLessonRepo.DeleteAsync(id);
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
