using UserAPI.Application.DTOs.Request.UserInputAnswer;
using UserAPI.Application.DTOs.Response.UserInputAnswer;

namespace UserAPI.Application.Interfaces.IServices
{
    public interface IUserInputAnswerService
    {
        Task<IEnumerable<UserInputAnswerResponse>> GetAllUserInputAnswersAsync();
        Task<UserInputAnswerResponse?> GetUserInputAnswerByIdAsync(Guid id);
        Task<IEnumerable<UserInputAnswerResponse>> GetUserInputAnswersByResultIdAsync(Guid resultId);
        Task<UserInputAnswerResponse?> GetUserInputAnswerByResultAndQuestionIdAsync(Guid resultId, Guid questionId);
        Task<UserInputAnswerResponse> CreateUserInputAnswerAsync(CreateUserInputAnswerRequest request);
        Task SaveUserInputAnswer(CreateUserInputAnswerRequest request);
        Task<UserInputAnswerResponse?> UpdateUserInputAnswerAsync(Guid id, UpdateUserInputAnswerRequest request);
        Task<bool> DeleteUserInputAnswerAsync(Guid id);
    }
}
