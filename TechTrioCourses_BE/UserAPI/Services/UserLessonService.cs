using AutoMapper;
using UserAPI.DTOs.Request;
using UserAPI.DTOs.Response;
using UserAPI.Enums;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class UserLessonService : IUserLessonService
    {
        private readonly IUserLessonRepo _userLessonRepo;
        private readonly IMapper _mapper;

        public UserLessonService(IUserLessonRepo userLessonRepo, IMapper mapper)
        {
            _userLessonRepo = userLessonRepo;
            _mapper = mapper;
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
            userLesson.Status = UserLessonStatus.Completed;
            userLesson.CompletedAt = DateTime.UtcNow;

            var createdUserLesson = await _userLessonRepo.CreateUserLessonAsync(userLesson);

            return _mapper.Map<UserLessonResponse>(createdUserLesson);
        }

        public async Task<UserLessonResponse?> MarkLessonAsCompleteAsync(Guid id)
        {
            var userLesson = await _userLessonRepo.GetByIdAsync(id);
            if (userLesson == null)
            {
                return null;
            }

            // Mark lesson as completed
            userLesson.Status = UserLessonStatus.Completed;
            userLesson.CompletedAt = DateTime.UtcNow;

            await _userLessonRepo.UpdateUserLessonAsync(userLesson);

            return _mapper.Map<UserLessonResponse>(userLesson);
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
