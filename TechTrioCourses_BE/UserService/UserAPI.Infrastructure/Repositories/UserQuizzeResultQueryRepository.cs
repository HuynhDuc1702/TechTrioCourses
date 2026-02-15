using Microsoft.EntityFrameworkCore;
using UserAPI.Application.DTOs.Projections.AttemptUserQuizzeResultDetailProjections;
using UserAPI.Infrastructure.Data;

using UserAPI.Application.Interfaces.IRepositories;

namespace UserAPI.Infrastructure.Repositories
{
    public class UserQuizzeResultQueryRepository : IUserQuizzeResultQueryRepository
    {
        private readonly UserDbContext _context;

        public UserQuizzeResultQueryRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task <UserQuizzeResultDetailResponseProjection?> GetUserQuizzeResultDetailAsync (Guid id)
        {
            return await _context.UserQuizzeResults
                .Where(r => r.Id == id)
                .Select(r => new UserQuizzeResultDetailResponseProjection
                {
                    ResultId = r.Id,
                    QuizId= r.QuizId,
                   UserQuizId=r.UserQuizId,
                    AttemptNumber= r.AttemptNumber,
                    Status= r.Status,
                    Score= r.Score,
                    StartedAt= r.StartedAt,
                    CompletedAt= r.CompletedAt,

                    Answers = r.UserInputAnswers
                    .Select (a=> new UserQuizzeResultQuestionAnswerProjection
                    {
                        QuestionId = a.QuestionId,
                        TextAnswer=a.AnswerText,


                        SelectedChoiceIds= r.UserSelectedChoices
                        .Where(sl=> sl.QuestionId==a.QuestionId)
                        .Select(c=> c.ChoiceId)
                        .ToList()
                    }).ToList()


                }).AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
