using AutoMapper;
using QuizAPI.DTOs.Request.Quiz;
using QuizAPI.DTOs.Response.AttemptQuizDetailDTOs;
using QuizAPI.DTOs.Response.Quiz;
using QuizAPI.Models;
using QuizAPI.Repositories.Interfaces;
using QuizAPI.Services.Interfaces;
using TechTrioCourses.Shared.Enums;

namespace QuizAPI.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepo _quizRepo;
        private readonly IMapper _mapper;
        private readonly IQuizQuery _quizQuery;

        public QuizService(IQuizRepo quizRepo, IMapper mapper, IQuizQuery quizQuery)
        {
            _quizRepo = quizRepo;
            _mapper = mapper;
            _quizQuery = quizQuery;
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

        public async Task<IEnumerable<QuizResponse>> GetQuizzesByCourseIdAsync(Guid courseId)
        {
            var quizzes = await _quizRepo.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<QuizResponse>>(quizzes);
        }

        public async Task<QuizResponse> CreateQuizAsync(CreateQuizRequest request)
        {
            var quiz = _mapper.Map<Quiz>(request);

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
        public async Task <QuizDetailResponseDto?> GetQuizDetailForAttemptAsync( Guid quizId)
        {
             var projection = await _quizQuery.GetQuizDetailForAttemptAsync(quizId); 
            if(projection == null) return null;

            return new QuizDetailResponseDto
            {
                Id = projection.Id,
                Name = projection.Name,
                Description = projection.Description,
                DurationMinutes = projection.DurationMinutes,

                Questions = projection.Questions.Select(
                        q=> new QuizQuestionDto
                        {
                            QuestionId=q.QuestionId,
                            QuestionText=q.QuestionText,
                            QuestionType  =q.QuestionType,
                            Points = q.Points,
                            Order=q.Order,

                            Choices= q.Choices?.Select(c=> new QuestionChoiceDto
                            {
                                Id=c.Id,
                                ChoiceText=c.ChoiceText,
                            }).ToList(),
                        }
                    ).ToList(),

            };
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

            if (existingQuiz.Status == PublishStatusEnum.Hidden)
            {
                return true; // Already disabled, no need to update
            }

            existingQuiz.Status = PublishStatusEnum.Hidden;
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

            if (existingQuiz.Status == PublishStatusEnum.Archived)
            {
                return true; // Already archived, no need to update
            }

            existingQuiz.Status = PublishStatusEnum.Archived;
            existingQuiz.UpdatedAt = DateTime.UtcNow;
            var updatedQuiz = await _quizRepo.UpdateAsync(existingQuiz);

            return updatedQuiz != null;
        }
    }
}
