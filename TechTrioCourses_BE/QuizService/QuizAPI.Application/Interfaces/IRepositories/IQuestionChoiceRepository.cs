using QuizAPI.Domain.Entities;
using TechTrioCourses.Shared.Repositories.Interfaces;

namespace QuizAPI.Application.Interfaces.IRepositories
{
    public interface IQuestionChoiceRepository : IGenericRepository <QuestionChoice>
    {
    
        Task<IEnumerable<QuestionChoice>> GetByQuestionIdAsync(Guid questionId);
        
  
    }
}
