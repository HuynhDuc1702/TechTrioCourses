using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using TechTrioCourses.Shared.Enums;
using UserAPI.Application.DTOs.Request.UserQuiz;
using UserAPI.Application.DTOs.Response.UserQuiz;
using UserAPI.Application.Interfaces.IRepositories;
using UserAPI.Application.Interfaces.IServices;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Services
{
    public class UserQuizService : IUserQuizService
    {
        private readonly IUserQuizRepository _userQuizRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UserQuizService> _logger;
        private readonly IUserCourseProgressService _userCourseProgress;
        public UserQuizService(IUserQuizRepository userQuizRepo, IMapper mapper, ILogger<UserQuizService> logger, IUserCourseProgressService userCourseProgress)
        {
            _userQuizRepo = userQuizRepo;
            _mapper = mapper;
            _logger = logger;
            _userCourseProgress = userCourseProgress;
        }

        public async Task<UserQuizResponse?> GetUserQuizByIdAsync(Guid id)
        {
            var userQuiz = await _userQuizRepo.GetByIdAsync(id);
            return userQuiz == null ? null : _mapper.Map<UserQuizResponse>(userQuiz);
        }

        public async Task<IEnumerable<UserQuizResponse>> GetAllUserQuizzesAsync()
        {
            var userQuizzes = await _userQuizRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserQuizResponse>>(userQuizzes);
        }

        public async Task<IEnumerable<UserQuizResponse>> GetUserQuizzesByUserIdAsync(Guid userId)
        {
            var userQuizzes = await _userQuizRepo.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<UserQuizResponse>>(userQuizzes);
        }

        public async Task<IEnumerable<UserQuizResponse>> GetUserQuizzesByQuizIdAsync(Guid quizId)
        {
            var userQuizzes = await _userQuizRepo.GetByQuizIdAsync(quizId);
            return _mapper.Map<IEnumerable<UserQuizResponse>>(userQuizzes);
        }

        public async Task<IEnumerable<UserQuizResponse>> GetUserQuizzesByCourseIdAsync(Guid courseId)
        {
            var userQuizzes = await _userQuizRepo.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<UserQuizResponse>>(userQuizzes);
        }

        public async Task<IEnumerable<UserQuizResponse>> GetUserQuizzesByUserAndCourseAsync(Guid userId, Guid courseId)
        {
            var userQuizzes = await _userQuizRepo.GetByUserAndCourseAsync(userId, courseId);
            return _mapper.Map<IEnumerable<UserQuizResponse>>(userQuizzes);
        }

        public async Task<UserQuizResponse?> GetUserQuizByUserAndQuizAsync(Guid userId, Guid quizId)
        {
            var userQuiz = await _userQuizRepo.GetByUserAndQuizAsync(userId, quizId);
            return userQuiz == null ? null : _mapper.Map<UserQuizResponse>(userQuiz);
        }

        public async Task<UserQuizResponse?> CreateUserQuizAsync(CreateUserQuizRequest request)
        {
            if (await _userQuizRepo.ExistsAsync(request.UserId, request.QuizId))
            {
                return null;
            }

            var now = DateTime.UtcNow;

            var userQuiz = _mapper.Map<UserQuiz>(request);
            userQuiz.Id = Guid.NewGuid();
            userQuiz.FirstAttemptAt = now;
            userQuiz.LastAttemptAt = now;
            userQuiz.AttemptCount = 1;
            userQuiz.Status = UserQuizStatusEnum.In_progress;

            var createdUserQuiz = await _userQuizRepo.CreateAsync(userQuiz);

            return _mapper.Map<UserQuizResponse>(createdUserQuiz);
        }


        public async Task<UserQuizResponse?> UpdateUserQuizAsync(Guid id, ApplyQuizGradingResultRequest request)
        {
           
            var userQuiz = await _userQuizRepo.GetByIdAsync(id);
            var now= DateTime.UtcNow;
            if (userQuiz == null)
            {
                return null;
            }
            if (!userQuiz.BestScore.HasValue || request.SubmitScore > userQuiz.BestScore)
            {
                userQuiz.BestScore = request.SubmitScore;
            }
            if (userQuiz.Status != UserQuizStatusEnum.Passed) {
                if (request.IsPassed)
                {
                    userQuiz.Status= UserQuizStatusEnum.Passed;
                    userQuiz.PassedAt = now;
              
                    try

                    {
                        var courseId = userQuiz.CourseId
                        ?? throw new InvalidOperationException("Passed quiz must have CourseId");

                        await _userCourseProgress.RecaculateCourseProgress(courseId, userQuiz.UserId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error during course progress recalculation for CourseId: {CourseId}, UserId: {UserId}",
                          userQuiz.CourseId, userQuiz.UserId);
                    }
                }
                else
                {
                    userQuiz.Status = UserQuizStatusEnum.Failed;
                }
            }
            userQuiz.UpdatedAt = now;



            var updatedUserQuiz = await _userQuizRepo.UpdateAsync(userQuiz);


            return _mapper.Map<UserQuizResponse>(updatedUserQuiz);
        }
        public async Task<UserQuizResponse?> RetakeUserQuizAsync(Guid id)
        {
            var userQuiz = await _userQuizRepo.GetByIdAsync(id);
            if (userQuiz == null)
            {
                return null;
            }
            userQuiz.AttemptCount += 1;
            userQuiz.LastAttemptAt = DateTime.UtcNow;

            await _userQuizRepo.UpdateAsync(userQuiz);

            return _mapper.Map<UserQuizResponse>(userQuiz);
        }


        public async Task<bool> DeleteUserQuizAsync(Guid id)
        {
            return await _userQuizRepo.DeleteAsync(id);
        }
    }
}
