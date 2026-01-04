using UserAPI.DTOs.Request.UserSelectedChoice;
using UserAPI.DTOs.Response.UserSelectedChoice;

namespace UserAPI.Services.Interfaces
{
    public interface IUserSelectedChoiceService
    {
        Task<IEnumerable<UserSelectedChoiceResponse>> GetAllUserSelectedChoicesAsync();
        Task<UserSelectedChoiceResponse?> GetUserSelectedChoiceByIdAsync(Guid id);
 Task<IEnumerable<UserSelectedChoiceResponse>> GetUserSelectedChoicesByResultIdAsync(Guid resultId);
    Task<UserSelectedChoiceResponse?> GetUserSelectedChoiceByResultAndQuestionIdAsync(Guid resultId, Guid questionId);
  Task<UserSelectedChoiceResponse> CreateUserSelectedChoiceAsync(CreateUserSelectedChoiceRequest request);
  Task<UserSelectedChoiceResponse?> UpdateUserSelectedChoiceAsync(Guid id, UpdateUserSelectedChoiceRequest request);
 Task<bool> DeleteUserSelectedChoiceAsync(Guid id);
    }
}
