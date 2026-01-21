using AutoMapper;
using System.Reflection.Metadata.Ecma335;
using TechTrioCourses.Shared.Enums;
using UserAPI.DTOs.Request.UserQuiz;
using UserAPI.DTOs.Response.UserQuiz;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class UserQuizService : IUserQuizService
    {
        private readonly IUserQuizRepo _userQuizRepo;
        private readonly IMapper _mapper;

        public UserQuizService(IUserQuizRepo userQuizRepo, IMapper mapper)
        {
            _userQuizRepo = userQuizRepo;
            _mapper = mapper;
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
            if (await _userQuizRepo.UserQuizExistsAsync(request.UserId, request.QuizId))
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

            var createdUserQuiz = await _userQuizRepo.CreateUserQuizAsync(userQuiz);

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
                }
                else
                {
                    userQuiz.Status = UserQuizStatusEnum.Failed;
                }
            }
            userQuiz.UpdatedAt = now;



            var updatedUserQuiz = await _userQuizRepo.UpdateUserQuizAsync(userQuiz);


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

            await _userQuizRepo.UpdateUserQuizAsync(userQuiz);

            return _mapper.Map<UserQuizResponse>(userQuiz);
        }


        public async Task<bool> DeleteUserQuizAsync(Guid id)
        {
            return await _userQuizRepo.DeleteUserQuizAsync(id);
        }
    }
}
