using UserAPI.Models;

namespace UserAPI.Repositories.Interfaces
{
    public interface IUserInputAnswerRepo
  {
    Task<IEnumerable<UserInputAnswer>> GetAllAsync();
 Task<UserInputAnswer?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserInputAnswer>> GetByResultIdAsync(Guid resultId);
        Task<UserInputAnswer?> GetByResultAndQuestionIdAsync(Guid resultId, Guid questionId);
   Task<UserInputAnswer> CreateAsync(UserInputAnswer userInputAnswer);
        Task<UserInputAnswer?> UpdateAsync(UserInputAnswer userInputAnswer);
  Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
