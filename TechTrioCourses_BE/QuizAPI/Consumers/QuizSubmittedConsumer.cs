using MassTransit;
using QuizAPI.DTOs.Request.GradeQuizDTOs;
using QuizAPI.Services.Interfaces;
using TechTrioCourses.Shared.Contracts;

namespace QuizAPI.Consumers
{
    public class QuizSubmittedConsumer : IConsumer<QuizSubmittedEvent>
    {

        private readonly ILogger<QuizSubmittedConsumer> _logger;
        private readonly IQuizService _quizService;

        public QuizSubmittedConsumer(ILogger<QuizSubmittedConsumer> logger, IQuizService quizService)
        {
            _logger = logger;
            _quizService = quizService;
        }

        public async Task Consume(ConsumeContext<QuizSubmittedEvent> context)
        {
            var evt = context.Message;

            _logger.LogInformation(
            "Grading quiz {QuizId} , result {ResultId}",
            evt.QuizId, evt.ResultId);
            try
            {
                var gradingResult = await _quizService.GradeQuizAsync(new GradeQuizRequestDto
                {
                    QuizId = evt.QuizId,
                    Answers = evt.Answers.Select(a => new UserQuestionAnswersDtos
                    {
                        QuestionId = a.QuestionId,
                        QuestionType = a.QuestionType,
                        SelectedChoices = a.SelectedChoices,
                        TextAnswer = a.TextAnswer,
                    }).ToList(),
                });
                if (gradingResult == null)
                {
                    _logger.LogWarning(
                        "Grading quiz {QuizId} failed: quiz not found",
                        evt.QuizId);

                    return; 
                }

                await context.Publish(new QuizGradedEvent
                {
                    QuizId = evt.QuizId,
                    ResultId = evt.ResultId,
                    UserQuizId = evt.UserQuizId,
                    TotalPointsEarned = gradingResult.TotalPointsEarned,
                    TotalMarks = gradingResult.TotalMarks,
                    PercentageScore = gradingResult.PercentageScore,
                    IsPassed = gradingResult.IsPassed,
                    GradedAt=DateTime.UtcNow,
                    GradedQuestions = gradingResult.GradedQuestions.Select(q => new GradedQuestionEvent
                    {
                        QuestionId = q.QuestionId,
                        IsCorrect = q.IsCorrect,
                        PointsEarned = q.PointsEarned,
                        MaxPoints = q.MaxPoints,
                    }).ToList(),
                });

                _logger.LogInformation(
           "Graded quiz {QuizId} sucessful , result {ResultId}",
           evt.QuizId, evt.ResultId);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex,
                    "Error grading quiz {QuizId} for result {ResultId}",
                    evt.QuizId, evt.ResultId);

              
                throw;
            }
        }
    }
}
