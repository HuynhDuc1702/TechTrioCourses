using AutoMapper;
using QuizAPI.DTOs.Request.Question;
using QuizAPI.DTOs.Response.Question;
using QuizAPI.Enums;
using QuizAPI.Models;
using QuizAPI.Repositories;
using QuizAPI.Repositories.Interfaces;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepo _questionRepo;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepo questionRepo, IMapper mapper)
        {
            _questionRepo = questionRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync()
        {
            var questions = await _questionRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<QuestionResponse>>(questions);
        }

        public async Task<QuestionResponse?> GetQuestionByIdAsync(Guid id)
        {
            var question = await _questionRepo.GetByIdAsync(id);

            if (question == null)
            {
                return null;
            }

            return _mapper.Map<QuestionResponse>(question);
        }

        public async Task<QuestionResponse> CreateQuestionAsync(CreateQuestionRequest request)
        {
            var question = _mapper.Map<Question>(request);
            

            var createdQuestion = await _questionRepo.CreateAsync(question);

            return _mapper.Map<QuestionResponse>(createdQuestion);
        }

        public async Task<QuestionResponse?> UpdateQuestionAsync(Guid id, UpdateQuestionRequest request)
        {
            var existingQuestion = await _questionRepo.GetByIdAsync(id);

            if (existingQuestion == null)
            {
                return null;
            }

            // Map only non-null properties from request to existing question
            if (request.QuestionText != null)
                existingQuestion.QuestionText = request.QuestionText;

            if (request.QuestionType.HasValue)
                existingQuestion.QuestionType = request.QuestionType.Value;

            if (request.Status.HasValue)
                existingQuestion.Status = request.Status.Value;

            if (request.Points.HasValue)
                existingQuestion.Points = request.Points.Value;

    
            var updatedQuestion = await _questionRepo.UpdateAsync(existingQuestion);

            if (updatedQuestion == null)
            {
                return null;
            }

            return _mapper.Map<QuestionResponse>(updatedQuestion);
        }

        public async Task<bool> DeleteQuestionAsync(Guid id)
        {
            return await _questionRepo.DeleteAsync(id);
        }
        public async Task<bool> DisableQuestionAsync(Guid id)
        {
            var existingQuestion = await _questionRepo.GetByIdAsync(id);

            if (existingQuestion == null)
            {
                return false;
            }

            if (existingQuestion.Status == QuestionStatusEnum.Hidden)
            {
                return true; // Already disabled, no need to update
            }

            existingQuestion.Status = QuestionStatusEnum.Hidden;
            existingQuestion.UpdatedAt = DateTime.UtcNow;
            var updatedQuestion = await _questionRepo.UpdateAsync(existingQuestion);

            return updatedQuestion != null;
        }

        public async Task<bool> ArchiveQuestionAsync(Guid id)
        {
            var existingQuestion = await _questionRepo.GetByIdAsync(id);

            if (existingQuestion == null)
            {
                return false;
            }

            if (existingQuestion.Status == QuestionStatusEnum.Archived)
            {
                return true; // Already disabled, no need to update
            }

            existingQuestion.Status = QuestionStatusEnum.Archived;
            existingQuestion.UpdatedAt = DateTime.UtcNow;
            var updatedQuestion = await _questionRepo.UpdateAsync(existingQuestion);

            return updatedQuestion != null;
        }
    }
}

