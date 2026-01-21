using MassTransit;
using System.Text.Json;
using TechTrioCourses.Shared.Contracts;
using TechTrioCourses.Shared.Enums;
using UserAPI.DTOs.Request.SubmitQuizDTOs;
using UserAPI.DTOs.Request.UserQuiz;
using UserAPI.DTOs.Request.UserQuizzeResult;
using UserAPI.Services.Interfaces;

namespace UserAPI.Consumers
{
    public class QuizGradedConsumer : IConsumer<QuizGradedEvent>
    {
        private readonly ILogger<QuizGradedConsumer> _logger;
        private readonly IUserQuizzeResultService _service;
        private readonly IUserQuizService _userQuizService;

        public QuizGradedConsumer(ILogger<QuizGradedConsumer> logger, IUserQuizzeResultService service, IUserQuizService userQuizService)
        {
            _logger = logger;
            _service = service;
            _userQuizService = userQuizService;
        }

        public async Task Consume(ConsumeContext<QuizGradedEvent> context)
        {
            var evt = context.Message;

            _logger.LogInformation(
                "Received grading result for {ResultId}. Score: {Score}/{MaxScore}, Passed: {Passed}",
                evt.ResultId,
                evt.TotalPointsEarned,
                evt.TotalMarks,
                evt.IsPassed);

            try
            {
                await _service.UpdateQuizzeResultAsync(
                    evt.ResultId,
                    new UpdateUserQuizzeResultRequest
                    {
                        Score = evt.TotalPointsEarned,
                        IsPassed=evt.IsPassed,
                        CompletedAt = DateTime.UtcNow,
                    });

                await _userQuizService.UpdateUserQuizAsync(
                    evt.UserQuizId,
                    new ApplyQuizGradingResultRequest
                    {
                        IsPassed = evt.IsPassed,
                        SubmitScore = evt.TotalPointsEarned,
                    });

                _logger.LogInformation(
                    "Quiz result {ResultId} and User Quiz {UserQuizId} updated successfully",
                    evt.ResultId, evt.UserQuizId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error updating quiz result {ResultId} and User Quiz {UserQuizId}",
                    evt.ResultId, evt.UserQuizId);

                throw;
            }
        }

    }
}
