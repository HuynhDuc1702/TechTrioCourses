using AutoMapper;
using Microsoft.Extensions.Logging;
using TechTrioCourses.Shared.Enums;
using UserAPI.Application.DTOs.Request.UserCourse;
using UserAPI.Application.DTOs.Response.UserCourse;
using UserAPI.Application.Interfaces.IRepositories;
using UserAPI.Application.Interfaces.IServices;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Services
{
    public class UserCourseService : IUserCourseService
    {
        private readonly IUserCourseRepository _userCourseRepo;
    
        private readonly IMapper _mapper;
        
        private readonly ILogger<UserCourseService> _logger;
        public UserCourseService(IUserCourseRepository userCourseRepo, IMapper mapper, ILogger<UserCourseService> logger)
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
            if (await _userCourseRepo.ExistsAsync(request.UserId, request.CourseId))
            {
                return null;
            }

            
            var userCourse = _mapper.Map<UserCourse>(request);
            userCourse.Progress = 0;
            userCourse.Status = UserCourseStatusEnum.In_progress;
            userCourse.EnrolledAt=DateTime.UtcNow;
            var createdUserCourse = await _userCourseRepo.CreateAsync(userCourse);

            return _mapper.Map<UserCourseResponse>(createdUserCourse);
        }

        public async Task<UserCourseResponse?> UpdateUserCourseAsync(Guid id)
        {
            var userCourse = await _userCourseRepo.GetByIdAsync(id);
            if (userCourse == null)
            {
                return null;
            }



            var updatedUserCourse = await _userCourseRepo.UpdateAsync(userCourse);

            if (updatedUserCourse == null)
            {
                return null; 
            }


            return _mapper.Map<UserCourseResponse>(updatedUserCourse);
        }

        public async Task<bool> DeleteUserCourseAsync(Guid id)
        {
            return await _userCourseRepo.DeleteAsync(id);
        }
    }
}
