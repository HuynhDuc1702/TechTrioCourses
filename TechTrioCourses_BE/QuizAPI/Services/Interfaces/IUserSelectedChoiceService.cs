using QuizAPI.DTOs.Request.UserSelectedChoice;
using QuizAPI.DTOs.Response.UserSelectedChoice;

namespace QuizAPI.Services.Interfaces
{
    public interface IUserSelectedChoiceService
 {
   Task<IEnumerable<UserSelectedChoiceResponse>> GetAllUserSelectedChoicesAsync();
 Task<UserSelectedChoiceResponse?> GetUserSelectedChoiceByIdAsync(Guid id);
  Task<IEnumerable<UserSelectedChoiceResponse>> GetUserSelectedChoicesByResultIdAsync(Guid resultId);
      Task<UserSelectedChoiceResponse> CreateUserSelectedChoiceAsync(CreateUserSelectedChoiceRequest request);
 Task<UserSelectedChoiceResponse?> UpdateUserSelectedChoiceAsync(Guid id, UpdateUserSelectedChoiceRequest request);
     Task<bool> DeleteUserSelectedChoiceAsync(Guid id);
    }
}
