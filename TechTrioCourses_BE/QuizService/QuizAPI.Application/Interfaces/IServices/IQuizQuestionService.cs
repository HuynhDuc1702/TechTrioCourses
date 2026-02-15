using QuizAPI.Application.DTOs.Request.QuizQuestion;
using QuizAPI.Application.DTOs.Response.QuizQuestion;

namespace QuizAPI.Application.Interfaces.IServices
{
    public interface IQuizQuestionService
    {
        Task<IEnumerable<QuizQuestionResponse>> GetAllQuizQuestionsAsync();
        Task<QuizQuestionResponse?> GetQuizQuestionByIdAsync(Guid quizId, Guid questionId);
        Task<IEnumerable<QuizQuestionResponse>> GetQuizQuestionsByQuizIdAsync(Guid quizId);
        Task<IEnumerable<QuizQuestionResponse>> GetQuizQuestionsByQuestionIdAsync(Guid questionId);
        Task<QuizQuestionResponse> CreateQuizQuestionAsync(CreateQuizQuestionRequest request);
        Task<QuizQuestionResponse?> UpdateQuizQuestionAsync(Guid quizId, Guid questionId, UpdateQuizQuestionRequest request);
        Task<bool> DeleteQuizQuestionAsync(Guid quizId, Guid questionId);
    }
}
