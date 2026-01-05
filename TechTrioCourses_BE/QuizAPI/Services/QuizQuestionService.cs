using AutoMapper;
using QuizAPI.DTOs.Request.QuizQuestion;
using QuizAPI.DTOs.Response.QuizQuestion;
using QuizAPI.Models;
using QuizAPI.Repositories.Interfaces;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Services
{
    public class QuizQuestionService : IQuizQuestionService
  {
        private readonly IQuizQuestionRepo _quizQuestionRepo;
        private readonly IMapper _mapper;

        public QuizQuestionService(IQuizQuestionRepo quizQuestionRepo, IMapper mapper)
        {
            _quizQuestionRepo = quizQuestionRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuizQuestionResponse>> GetAllQuizQuestionsAsync()
        {
        var quizQuestions = await _quizQuestionRepo.GetAllAsync();
   return _mapper.Map<IEnumerable<QuizQuestionResponse>>(quizQuestions);
        }

        public async Task<QuizQuestionResponse?> GetQuizQuestionByIdAsync(Guid quizId, Guid questionId)
        {
        var quizQuestion = await _quizQuestionRepo.GetByIdAsync(quizId, questionId);

         if (quizQuestion == null)
            {
       return null;
            }

            return _mapper.Map<QuizQuestionResponse>(quizQuestion);
        }

        public async Task<IEnumerable<QuizQuestionResponse>> GetQuizQuestionsByQuizIdAsync(Guid quizId)
   {
            var quizQuestions = await _quizQuestionRepo.GetByQuizIdAsync(quizId);
         return _mapper.Map<IEnumerable<QuizQuestionResponse>>(quizQuestions);
        }

        public async Task<IEnumerable<QuizQuestionResponse>> GetQuizQuestionsByQuestionIdAsync(Guid questionId)
   {
         var quizQuestions = await _quizQuestionRepo.GetByQuestionIdAsync(questionId);
     return _mapper.Map<IEnumerable<QuizQuestionResponse>>(quizQuestions);
        }

        public async Task<QuizQuestionResponse> CreateQuizQuestionAsync(CreateQuizQuestionRequest request)
        {
        var quizQuestion = _mapper.Map<QuizQuestion>(request);

    var createdQuizQuestion = await _quizQuestionRepo.CreateAsync(quizQuestion);

       return _mapper.Map<QuizQuestionResponse>(createdQuizQuestion);
        }

      public async Task<QuizQuestionResponse?> UpdateQuizQuestionAsync(Guid quizId, Guid questionId, UpdateQuizQuestionRequest request)
        {
   var existingQuizQuestion = await _quizQuestionRepo.GetByIdAsync(quizId, questionId);

   if (existingQuizQuestion == null)
  {
         return null;
            }

            // Map only non-null properties from request to existing quizQuestion
    if (request.QuestionOrder.HasValue)
   existingQuizQuestion.QuestionOrder = request.QuestionOrder;

    if (request.OverridePoints.HasValue)
      existingQuizQuestion.OverridePoints = request.OverridePoints;

  var updatedQuizQuestion = await _quizQuestionRepo.UpdateAsync(existingQuizQuestion);

    if (updatedQuizQuestion == null)
       {
    return null;
            }

   return _mapper.Map<QuizQuestionResponse>(updatedQuizQuestion);
        }

        public async Task<bool> DeleteQuizQuestionAsync(Guid quizId, Guid questionId)
        {
      return await _quizQuestionRepo.DeleteAsync(quizId, questionId);
        }
    }
}
