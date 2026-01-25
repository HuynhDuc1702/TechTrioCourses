using Microsoft.EntityFrameworkCore;
using UserAPI.Datas;
using UserAPI.DTOs.Projections.AttemptUserQuizzeResultDetailProjections;

using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class UserQuizzeResultQueryRepo : IUserQuizzeResultQueryRepo
    {
        private readonly TechTrioUsersContext _context;

        public UserQuizzeResultQueryRepo(TechTrioUsersContext context)
        {
            _context = context;
        }

        public async Task <UserQuizzeResultDetailResponseProjection?> GetUserQuizzeResultDetailForAttemptAsync (Guid id)
        {
            return await _context.UserQuizzeResults
                .Where(r => r.Id == id)
                .Select(r => new UserQuizzeResultDetailResponseProjection
                {
                    ResultId = r.Id,
                    QuizId= r.QuizId,
                   
                    AttemptNumber= r.AttemptNumber,
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
