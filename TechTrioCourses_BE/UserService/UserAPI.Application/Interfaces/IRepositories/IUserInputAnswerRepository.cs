using TechTrioCourses.Shared.Repositories.Interfaces;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Interfaces.IRepositories
{
    public interface IUserInputAnswerRepository: IGenericRepository<UserInputAnswer>
    {
        Task<IEnumerable<UserInputAnswer>> GetByResultIdAsync(Guid resultId);
        Task<UserInputAnswer?> GetByResultAndQuestionIdAsync(Guid resultId, Guid questionId);
       
    }
}
