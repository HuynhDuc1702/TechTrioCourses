using AutoMapper;
using UserAPI.DTOs.Request.UserQuizzeResult;
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

        public UserQuizzeResultService(IUserQuizzeResultRepo quizzeResultRepo, IMapper mapper)
        {
            _quizzeResultRepo = quizzeResultRepo;
            _mapper = mapper;
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

        public async Task<UserQuizzeResultResponse> CreateQuizzeResultAsync(CreateUserQuizzeResultRequest request)
        {
            var quizzeResult = _mapper.Map<UserQuizzeResult>(request);
            var createdResult = await _quizzeResultRepo.CreateAsync(quizzeResult);
            return _mapper.Map<UserQuizzeResultResponse>(createdResult);
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
