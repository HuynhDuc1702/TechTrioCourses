using QuizAPI.Domain.Entities;
using TechTrioCourses.Shared.Repositories.Interfaces;

namespace QuizAPI.Application.Interfaces.IRepositories
{
    public interface IQuestionAnswerRepository : IGenericRepository<QuestionAnswer>
    {
      
        Task<IEnumerable<QuestionAnswer>> GetByQuestionIdAsync(Guid questionId);
        
    }
}
