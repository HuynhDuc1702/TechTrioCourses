using AutoMapper;
using TechTrioCourses.Shared.Enums;
using UserAPI.DTOs.Request.UserQuizzeResult;
using UserAPI.DTOs.Response.AttemptUserQuizzeResultDetailDTOs;
using UserAPI.DTOs.Response.UserQuizzeResult;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class UserQuizzeResultService : IUserQuizzeResultService
    {
        private readonly IUserQuizzeResultRepo _quizzeResultRepo;
        private readonly IMapper _mapper;
        private readonly IUserQuizzeResultQueryRepo _userQuizzeResultQueryRepo;
        public UserQuizzeResultService(IUserQuizzeResultRepo quizzeResultRepo, IMapper mapper, IUserQuizzeResultQueryRepo quizzeResultQueryRepo)
        {
            _quizzeResultRepo = quizzeResultRepo;
            _mapper = mapper;
            _userQuizzeResultQueryRepo = quizzeResultQueryRepo;
        }

        public async Task<IEnumerable<UserQuizzeResultResponse>> GetAllQuizzeResultsAsync()
        {
            var results = await _quizzeResultRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserQuizzeResultResponse>>(results);
        }

        public async Task<UserQuizzeResultResponse?> GetQuizzeResultByIdAsync(Guid id)
        {
            var result = await _quizzeResultRepo.GetByIdAsync(id);
            if (result == null)
            {
                return null;
            }
            return _mapper.Map<UserQuizzeResultResponse>(result);
        }

        public async Task<IEnumerable<UserQuizzeResultResponse>> GetQuizzeResultsByUserIdAsync(Guid userId)
        {
            var results = await _quizzeResultRepo.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<UserQuizzeResultResponse>>(results);
        }

        public async Task<IEnumerable<UserQuizzeResultResponse>> GetQuizzeResultsByQuizIdAsync(Guid quizId)
        {
            var results = await _quizzeResultRepo.GetByQuizIdAsync(quizId);
            return _mapper.Map<IEnumerable<UserQuizzeResultResponse>>(results);
        }

        public async Task<IEnumerable<UserQuizzeResultResponse>> GetQuizzeResultsByUserAndQuizIdAsync(Guid userId, Guid quizId)
        {
            var results = await _quizzeResultRepo.GetByUserAndQuizIdAsync(userId, quizId);
            return _mapper.Map<IEnumerable<UserQuizzeResultResponse>>(results);
        }

        public async Task<IEnumerable<UserQuizzeResultResponse>> GetQuizzeResultsByUserQuizIdAsync(Guid userQuizId)
        {
            var results = await _quizzeResultRepo.GetByUserQuizIdAsync(userQuizId);
            return _mapper.Map<IEnumerable<UserQuizzeResultResponse>>(results);
        }
        public async Task<UserQuizzeResultResponse> GetLatestUserQuizzeResult(Guid userQuizId)
        {
            var results = await _quizzeResultRepo.GetLatestByUserQuizIdAsync(userQuizId);
            return _mapper.Map<UserQuizzeResultResponse>(results);
        }

        public async Task<UserQuizzeResultResponse> CreateQuizzeResultAsync(CreateUserQuizzeResultRequest request)
        {

            var quizzeResult = _mapper.Map<UserQuizzeResult>(request);
            var latestQuizzeResult = await _quizzeResultRepo.GetLatestByUserQuizIdAsync(quizzeResult.UserQuizId);
            quizzeResult.AttemptNumber =
            latestQuizzeResult == null
           ? 1
           : latestQuizzeResult.AttemptNumber + 1;
           
            var createdResult = await _quizzeResultRepo.CreateAsync(quizzeResult);
            return _mapper.Map<UserQuizzeResultResponse>(createdResult);
        }
        public async Task<UserQuizzeResultReviewResponseDtos?> GetUserQuizzeResultDetailForAttemptReviewAsync(Guid id)
        {
            var projection = await _userQuizzeResultQueryRepo.GetUserQuizzeResultDetailForAttemptAsync(id);
            if (projection == null) return null;

            return new UserQuizzeResultReviewResponseDtos

            {
                ResultId = projection.ResultId,
                QuizId = projection.QuizId,
                UserId = projection.UserId,
                AttemptNumber = projection.AttemptNumber,
                Score = projection.Score,
                StartedAt = projection.StartedAt,
                CompletedAt = projection.CompletedAt,

                Answers = projection.Answers.Select(
                       a => new UserQuizzeResultQuestionAnswerDtos
                       {
                           QuestionId = a.QuestionId,
                           TextAnswer = a.TextAnswer,

                           SelectedChoiceIds = a.SelectedChoiceIds
                       }

                 ).ToList()
            };
        }
        public async Task<UserQuizzeResultResumeResponseDto?> GetUserQuizzeResultDetailForAttemptResumeAsync(Guid id)
        {
            var projection = await _userQuizzeResultQueryRepo.GetUserQuizzeResultDetailForAttemptAsync(id);
            if (projection == null) return null;

            return new UserQuizzeResultResumeResponseDto

            {
                ResultId = projection.ResultId,
                QuizId = projection.QuizId,
                UserId = projection.UserId,
                AttemptNumber = projection.AttemptNumber,
                StartedAt = projection.StartedAt,


                Answers = projection.Answers.Select(
                       a => new UserQuizzeResultQuestionAnswerDtos
                       {
                           QuestionId = a.QuestionId,
                           TextAnswer = a.TextAnswer,

                           SelectedChoiceIds = a.SelectedChoiceIds
                       }

                 ).ToList()
            };
        }
        public async Task<UserQuizzeResultResponse?> UpdateQuizzeResultAsync(Guid id, UpdateUserQuizzeResultRequest request)
        {
            var existingResult = await _quizzeResultRepo.GetByIdAsync(id);
            if (existingResult == null)
            {
                return null;
            }

            // Map only non-null properties from request to existing result
            if (request.Score.HasValue)
                existingResult.Score = request.Score.Value;

            if (request.Status.HasValue)
                existingResult.Status = request.Status.Value;

            if (request.CompletedAt.HasValue)
                existingResult.CompletedAt = request.CompletedAt.Value;

            if (request.DurationSeconds.HasValue)
                existingResult.DurationSeconds = request.DurationSeconds.Value;

            if (request.Metadata != null)
                existingResult.Metadata = request.Metadata;

            var updatedResult = await _quizzeResultRepo.UpdateAsync(existingResult);
            if (updatedResult == null)
            {
                return null;
            }

            return _mapper.Map<UserQuizzeResultResponse>(updatedResult);
        }

        public async Task<bool> DeleteQuizzeResultAsync(Guid id)
        {
            return await _quizzeResultRepo.DeleteAsync(id);
        }
    }
}
