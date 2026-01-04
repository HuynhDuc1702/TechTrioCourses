using AutoMapper;
using UserAPI.DTOs.Request.UserCourse;
using UserAPI.DTOs.Response.UserCourse;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class UserCourseService : IUserCourseService
    {
        private readonly IUserCourseRepo _userCourseRepo;
    
        private readonly IMapper _mapper;
        
        private readonly ILogger<UserCourseService> _logger;
        public UserCourseService(IUserCourseRepo userCourseRepo, IMapper mapper, ILogger<UserCourseService> logger)
        {
            _userCourseRepo = userCourseRepo;
            _mapper = mapper;
           
            _logger = logger;
           
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

          

            await _userCourseRepo.UpdateUserCourseAsync(userCourse);

            return _mapper.Map<UserCourseResponse>(userCourse);
        }

        public async Task<bool> DeleteUserCourseAsync(Guid id)
        {
            return await _userCourseRepo.DeleteUserCourseAsync(id);
        }
    }
}
