using QuizAPI.Models;

namespace QuizAPI.Repositories.Interfaces
{
    public interface IUserSelectedChoiceRepo
 {
     Task<IEnumerable<UserSelectedChoice>> GetAllAsync();
   Task<UserSelectedChoice?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserSelectedChoice>> GetByResultIdAsync(Guid resultId);
   Task<UserSelectedChoice> CreateAsync(UserSelectedChoice userSelectedChoice);
 Task<UserSelectedChoice?> UpdateAsync(UserSelectedChoice userSelectedChoice);
 Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
