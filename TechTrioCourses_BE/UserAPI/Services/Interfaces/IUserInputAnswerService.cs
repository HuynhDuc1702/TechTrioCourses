using UserAPI.DTOs.Request.UserInputAnswer;
using UserAPI.DTOs.Response.UserInputAnswer;

namespace UserAPI.Services.Interfaces
{
    public interface IUserInputAnswerService
    {
    Task<IEnumerable<UserInputAnswerResponse>> GetAllUserInputAnswersAsync();
   Task<UserInputAnswerResponse?> GetUserInputAnswerByIdAsync(Guid id);
        Task<IEnumerable<UserInputAnswerResponse>> GetUserInputAnswersByResultIdAsync(Guid resultId);
        Task<UserInputAnswerResponse?> GetUserInputAnswerByResultAndQuestionIdAsync(Guid resultId, Guid questionId);
        Task<UserInputAnswerResponse> CreateUserInputAnswerAsync(CreateUserInputAnswerRequest request);
Task<UserInputAnswerResponse?> UpdateUserInputAnswerAsync(Guid id, UpdateUserInputAnswerRequest request);
        Task<bool> DeleteUserInputAnswerAsync(Guid id);
    }
}
