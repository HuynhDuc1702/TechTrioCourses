using Microsoft.EntityFrameworkCore;
using QuizAPI.Datas;
using QuizAPI.Repositories.Interfaces;
using QuizAPI.DTOs.Projections.AttemptQuizDetailProjections;
using QuizAPI.DTOs.Projections.FullQuizDetailProjections;
namespace QuizAPI.Repositories
{
    public class QuizQueryRepo : IQuizQueryRepo
    {
        private readonly QuizzesContext _context;
        public QuizQueryRepo(QuizzesContext context)
        {
            _context = context;
        }

        public async Task<AttemptQuizDetailResponseProjection?> GetQuizDetailForAttemptAsync(Guid quizId)
        {
            return await _context.Quizzes
                .Where(q => q.Id == quizId)
                .Select(q => new AttemptQuizDetailResponseProjection
                {
                    Id = q.Id,
                    Name = q.Name,
                    Description = q.Description,
                    DurationMinutes = q.DurationMinutes,


                    Questions = q.QuizQuestions
                    .OrderBy(qq => qq.QuestionOrder)
                    .Select(qq => new AttemptQuizQuestionProjection
                    {
                        QuestionId = qq.QuestionId,
                        QuestionText = qq.Question.QuestionText,
                        QuestionType = qq.Question.QuestionType,
                        Points = qq.OverridePoints ?? qq.Question.Points,
                        Order = qq.QuestionOrder,

                        Choices = qq.Question.QuestionChoices

                                .Select(c => new AttemptQuestionChoiceProjection
                                {
                                    Id = c.Id,
                                    ChoiceText = c.ChoiceText
                                })
                                .ToList()
                    }).ToList(),

                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

        }
        public async Task<QuizDetailProjection?> GetQuizDetailAsync(Guid quizId)
        {
            return await _context.Quizzes
                .Where(q => q.Id == quizId)
                .Select(q => new QuizDetailProjection
                {
                    Id = q.Id,
                    Name = q.Name,
                    Description = q.Description,
                    DurationMinutes = q.DurationMinutes,
                    TotalMarks = q.TotalMarks,


                    Questions = q.QuizQuestions
                    .OrderBy(qq => qq.QuestionOrder)
                    .Select(qq => new QuizQuestionDetailProjection
                    {
                        QuestionId = qq.QuestionId,
                        QuestionText = qq.Question.QuestionText,
                        QuestionType = qq.Question.QuestionType,
                        Points = qq.OverridePoints ?? qq.Question.Points,
                        Order = qq.QuestionOrder,

                        Choices = qq.Question.QuestionChoices

                                .Select(c => new QuizQuestionChoicesDetailProjection
                                {
                                    Id = c.Id,
                                    ChoiceText = c.ChoiceText,
                                    IsCorrect = c.IsCorrect,
                                })
                                .ToList(),
                        Answers = qq.Question.QuestionAnswers
                        .Select(a => new QuizQuestionAnswersDetailProjection
                        {
                            Id = a.Id,
                            AnswerText = a.AnswerText,
                        }).ToList(),
                    }).ToList(),

                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

        }

    }
}
