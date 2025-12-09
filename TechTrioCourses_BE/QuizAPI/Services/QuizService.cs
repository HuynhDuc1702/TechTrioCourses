using AutoMapper;
using QuizAPI.DTOs.Request.Quiz;
using QuizAPI.DTOs.Response.Quiz;
using QuizAPI.Enums;
using QuizAPI.Models;
using QuizAPI.Repositories.Interfaces;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepo _quizRepo;
        private readonly IMapper _mapper;

        public QuizService(IQuizRepo quizRepo, IMapper mapper)
        {
            _quizRepo = quizRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuizResponse>> GetAllQuizzesAsync()
        {
            var quizzes = await _quizRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<QuizResponse>>(quizzes);
        }

        public async Task<QuizResponse?> GetQuizByIdAsync(Guid id)
        {
            var quiz = await _quizRepo.GetByIdAsync(id);

            if (quiz == null)
            {
                return null;
            }

            return _mapper.Map<QuizResponse>(quiz);
        }

        public async Task<QuizResponse> CreateQuizAsync(CreateQuizRequest request)
        {
            var quiz = _mapper.Map<Quiz>(request);
            quiz.Status = QuizzStatusEnum.Hidden;

            var createdQuiz = await _quizRepo.CreateAsync(quiz);

            return _mapper.Map<QuizResponse>(createdQuiz);
        }

        public async Task<QuizResponse?> UpdateQuizAsync(Guid id, UpdateQuizRequest request)
        {
            var existingQuiz = await _quizRepo.GetByIdAsync(id);

            if (existingQuiz == null)
            {
                return null;
            }

            // Map only non-null properties from request to existing quiz
            if (request.Name != null)
                existingQuiz.Name = request.Name;

            if (request.Description != null)
                existingQuiz.Description = request.Description;

            if (request.TotalMarks.HasValue)
                existingQuiz.TotalMarks = request.TotalMarks.Value;

            if (request.Status.HasValue)
                existingQuiz.Status = request.Status.Value;

            if (request.DurationMinutes.HasValue)
                existingQuiz.DurationMinutes = request.DurationMinutes.Value;

            var updatedQuiz = await _quizRepo.UpdateAsync(existingQuiz);

            if (updatedQuiz == null)
            {
                return null;
            }

            return _mapper.Map<QuizResponse>(updatedQuiz);
        }

        public async Task<bool> DeleteQuizAsync(Guid id)
        {
            return await _quizRepo.DeleteAsync(id);
        }

        public async Task<bool> DisableQuizAsync(Guid id)
        {
            var existingQuiz = await _quizRepo.GetByIdAsync(id);

            if (existingQuiz == null)
            {
                return false;
            }

            if (existingQuiz.Status == QuizzStatusEnum.Hidden)
            {
                return true; // Already disabled, no need to update
            }

            existingQuiz.Status = QuizzStatusEnum.Hidden;
            existingQuiz.UpdatedAt = DateTime.UtcNow;
            var updatedQuiz = await _quizRepo.UpdateAsync(existingQuiz);

            return updatedQuiz != null;
        }

        public async Task<bool> ArchiveQuizAsync(Guid id)
        {
            var existingQuiz = await _quizRepo.GetByIdAsync(id);

            if (existingQuiz == null)
            {
                return false;
            }

            if (existingQuiz.Status == QuizzStatusEnum.Archived)
            {
                return true; // Already archived, no need to update
            }

            existingQuiz.Status = QuizzStatusEnum.Archived;
            existingQuiz.UpdatedAt = DateTime.UtcNow;
            var updatedQuiz = await _quizRepo.UpdateAsync(existingQuiz);

            return updatedQuiz != null;
        }
    }
}
