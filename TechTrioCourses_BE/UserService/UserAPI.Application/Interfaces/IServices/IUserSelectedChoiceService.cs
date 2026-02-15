using UserAPI.Application.DTOs.Request.UserSelectedChoice;
using UserAPI.Application.DTOs.Response.UserSelectedChoice;

namespace UserAPI.Application.Interfaces.IServices
{
    public interface IUserSelectedChoiceService
    {
        Task<IEnumerable<UserSelectedChoiceResponse>> GetAllUserSelectedChoicesAsync();
        Task<UserSelectedChoiceResponse?> GetUserSelectedChoiceByIdAsync(Guid id);
        Task<IEnumerable<UserSelectedChoiceResponse>> GetUserSelectedChoicesByResultIdAsync(Guid resultId);
        Task<UserSelectedChoiceResponse?> GetUserSelectedChoiceByResultAndQuestionIdAsync(Guid resultId, Guid questionId);
        Task<UserSelectedChoiceResponse> CreateUserSelectedChoiceAsync(CreateUserSelectedChoiceRequest request);
        Task SaveUserSelectedChoice(CreateUserSelectedChoiceRequest request);
        Task<UserSelectedChoiceResponse?> UpdateUserSelectedChoiceAsync(Guid id, UpdateUserSelectedChoiceRequest request);
        Task<bool> DeleteUserSelectedChoiceAsync(Guid id);
    }
}
