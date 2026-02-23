using TechTrioCourses.Shared.Repositories.Interfaces;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Interfaces.IRepositories
{
    public interface IUserSelectedChoiceRepository :IGenericRepository<UserSelectedChoice>
    {
      
        Task<IEnumerable<UserSelectedChoice>> GetByResultIdAsync(Guid resultId);
        Task<UserSelectedChoice?> GetByResultAndQuestionIdAsync(Guid resultId, Guid questionId);
        
    }
}
