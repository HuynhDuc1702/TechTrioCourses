using Microsoft.EntityFrameworkCore;
using QuizAPI.Datas;
using QuizAPI.Repositories.Interfaces;
using QuizAPI.DTOs.Projections.AttemptQuizDetailProjections;
namespace QuizAPI.Repositories
{
    public class QuizQueryRepo : IQuizQuery
    {
        private readonly QuizzesContext _context;
        public QuizQueryRepo(QuizzesContext context)
        {
            _context = context;
        }

        public async Task<QuizDetailResponseProjection?> GetQuizDetailForAttemptAsync(Guid quizId)
        {
            return await _context.Quizzes
                .Where(q => q.Id == quizId)
                .Select(q => new QuizDetailResponseProjection
                {
                    Id = q.Id,
                    Name = q.Name,
                    Description = q.Description,
                    DurationMinutes = q.DurationMinutes,


                    Questions = q.QuizQuestions
                    .OrderBy(qq => qq.QuestionOrder)
                    .Select(qq => new QuizQuestionProjection
                    {
                        QuestionId = qq.QuestionId,
                        QuestionText = qq.Question.QuestionText,
                        QuestionType = qq.Question.QuestionType,
                        Points = qq.OverridePoints ?? qq.Question.Points,
                        Order = qq.QuestionOrder,

                        Choices = qq.Question.QuestionChoices
                                .Select(c => new QuestionChoiceProjection
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
    }
}
