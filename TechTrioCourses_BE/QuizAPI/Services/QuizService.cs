using AutoMapper;
using QuizAPI.DTOs.Request.GradeQuizDTOs;
using QuizAPI.DTOs.Request.Quiz;
using QuizAPI.DTOs.Response.AttemptQuizDetailDTOs;
using QuizAPI.DTOs.Response.FullQuizDetailDTOs;
using QuizAPI.DTOs.Response.Quiz;
using QuizAPI.Models;
using QuizAPI.Repositories.Interfaces;
using QuizAPI.Services.Interfaces;
using System.Configuration;
using System.Threading.Tasks;
using TechTrioCourses.Shared.Contracts;
using TechTrioCourses.Shared.Enums;

namespace QuizAPI.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepo _quizRepo;
        private readonly IMapper _mapper;
        private readonly IQuizQueryRepo _quizQuery;
        private readonly IQuestionChoiceService _questionChoiceService;
        private readonly IQuestionAnswerService _questionAnswerService;
        private readonly IQuestionService _questionService;

        public QuizService(IQuizRepo quizRepo, IMapper mapper, IQuizQueryRepo quizQuery, IQuestionChoiceService questionChoiceService, IQuestionAnswerService questionAnswerService, IQuestionService questionService
            )
        {
            _quizRepo = quizRepo;
            _mapper = mapper;
            _quizQuery = quizQuery;
            _questionChoiceService = questionChoiceService;
            _questionAnswerService = questionAnswerService;
            _questionService = questionService;
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
        public async Task<GradingResultDto> GradeQuizAsync(GradeQuizRequestDto request)
        {
            var quiz = await GetQuizByIdAsync(request.QuizId)
                ?? throw new Exception("Quiz not found");

            double totalPointsEarned = 0;
            var gradedQuestions = new List<GradedQuestionDto>();

            foreach (var quizQuestion in quiz.QuizQuestions)
            {
                var userAnswer = request.Answers
                    .FirstOrDefault(a => a.QuestionId == quizQuestion.QuestionId);

                var question = await _questionService.GetQuestionByIdAsync(quizQuestion.QuestionId);

                double maxPoints = quizQuestion.OverridePoints ?? question.Points;

                if (userAnswer == null)
                {
                    gradedQuestions.Add(new GradedQuestionDto
                    {
                        QuestionId = quizQuestion.QuestionId,
                        FeedBack = "Not answered",
                        PointsEarned = 0,
                        MaxPoints = maxPoints,
                        IsCorrect = false
                    });
                    continue;
                }

                bool isCorrect = await (question.QuestionType switch
                {
                    QuestionTypeEnum.Multiple_Choice or QuestionTypeEnum.True_False
                        => _questionChoiceService.GradeMultipleQuestion(userAnswer),

                    QuestionTypeEnum.Short_Answer
                        => _questionAnswerService.GradeShortAnswer(userAnswer),

                    _ => Task.FromResult(false)
                });

                double earned = isCorrect ? maxPoints : 0;

                totalPointsEarned += earned;

                gradedQuestions.Add(new GradedQuestionDto
                {
                    QuestionId = quizQuestion.QuestionId,
                    IsCorrect = isCorrect,
                    PointsEarned = earned,
                    MaxPoints = maxPoints,
                    FeedBack = isCorrect ? "Correct" : "Incorrect"
                });
            }

            double percentageScore = quiz.TotalMarks > 0
                ? (totalPointsEarned / quiz.TotalMarks) * 100
                : 0;

            return new GradingResultDto
            {
                TotalPointsEarned = totalPointsEarned,
                TotalMarks = quiz.TotalMarks,
                PercentageScore = percentageScore,
                IsPassed = percentageScore >= 80,
                GradedQuestions = gradedQuestions
            };
        }

        public async Task<AttemptQuizDetailResponseDto?> GetQuizDetailForAttemptAsync(Guid quizId)
        {
            var projection = await _quizQuery.GetQuizDetailForAttemptAsync(quizId);
            if (projection == null) return null;

            return new AttemptQuizDetailResponseDto
            {
                Id = projection.Id,
                Name = projection.Name,
                Description = projection.Description,
                DurationMinutes = projection.DurationMinutes,

                Questions = projection.Questions.Select(
                        q => new AttemptQuizQuestionDto
                        {
                            QuestionId = q.QuestionId,
                            QuestionText = q.QuestionText,
                            QuestionType = q.QuestionType,
                            Points = q.Points,
                            Order = q.Order,

                            Choices = q.Choices?.Select(c => new AttemptQuestionChoiceDto
                            {
                                Id = c.Id,
                                ChoiceText = c.ChoiceText,
                            }).ToList(),
                        }
                    ).ToList(),

            };
        }
        public async Task<QuizDetailResponseDto?> GetQuizDetailAsync(Guid quizId)
        {
            var projection = await _quizQuery.GetQuizDetailAsync(quizId);
            if (projection == null) return null;

            return new QuizDetailResponseDto
            {
                Id = projection.Id,
                Name = projection.Name,
                Description = projection.Description,
                TotalMarks = projection.TotalMarks,
                DurationMinutes = projection.DurationMinutes,

                Questions = projection.Questions.Select(
                        q => new QuizQuestionDetailDto
                        {
                            QuestionId = q.QuestionId,
                            QuestionText = q.QuestionText,
                            QuestionType = q.QuestionType,
                            Points = q.Points,
                            Order = q.Order,

                            Choices = q.Choices?.Select(c => new QuizQuestionChoicesDetailDto
                            {
                                Id = c.Id,
                                ChoiceText = c.ChoiceText,
                                IsCorrect = c.IsCorrect,
                            }).ToList(),
                            Answers = q.Answers?.Select(a => new QuizQuestionAnswersDetailDto
                            {
                                Id = a.Id,
                                AnswerText=a.AnswerText,
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
                return true; 
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
                return true; 
            }

            existingQuiz.Status = PublishStatusEnum.Archived;
            existingQuiz.UpdatedAt = DateTime.UtcNow;
            var updatedQuiz = await _quizRepo.UpdateAsync(existingQuiz);

            return updatedQuiz != null;
        }
    }
}
