using AutoMapper;
using QuizAPI.DTOs.Request.QuestionAnswer;
using QuizAPI.DTOs.Response.QuestionAnswer;
using QuizAPI.Models;
using QuizAPI.Repositories.Interfaces;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Services
{
    public class QuestionAnswerService : IQuestionAnswerService
{
    private readonly IQuestionAnswerRepo _questionAnswerRepo;
   private readonly IMapper _mapper;

      public QuestionAnswerService(IQuestionAnswerRepo questionAnswerRepo, IMapper mapper)
   {
      _questionAnswerRepo = questionAnswerRepo;
     _mapper = mapper;
        }

   public async Task<IEnumerable<QuestionAnswerResponse>> GetAllQuestionAnswersAsync()
     {
var answers = await _questionAnswerRepo.GetAllAsync();
    return _mapper.Map<IEnumerable<QuestionAnswerResponse>>(answers);
        }

  public async Task<QuestionAnswerResponse?> GetQuestionAnswerByIdAsync(Guid id)
  {
       var answer = await _questionAnswerRepo.GetByIdAsync(id);

if (answer == null)
 {
   return null;
   }

    return _mapper.Map<QuestionAnswerResponse>(answer);
 }

  public async Task<IEnumerable<QuestionAnswerResponse>> GetQuestionAnswersByResultIdAsync(Guid resultId)
 {
    var answers = await _questionAnswerRepo.GetByResultIdAsync(resultId);
     return _mapper.Map<IEnumerable<QuestionAnswerResponse>>(answers);
        }

 public async Task<QuestionAnswerResponse> CreateQuestionAnswerAsync(CreateQuestionAnswerRequest request)
 {
  var answer = _mapper.Map<QuestionAnswer>(request);

   var createdAnswer = await _questionAnswerRepo.CreateAsync(answer);

      return _mapper.Map<QuestionAnswerResponse>(createdAnswer);
 }

  public async Task<QuestionAnswerResponse?> UpdateQuestionAnswerAsync(Guid id, UpdateQuestionAnswerRequest request)
      {
   var existingAnswer = await _questionAnswerRepo.GetByIdAsync(id);

   if (existingAnswer == null)
{
return null;
  }

  // Map only non-null properties from request to existing answer
   if (request.AnswerText != null)
  existingAnswer.AnswerText = request.AnswerText;

      if (request.CorrectAnswer != null)
       existingAnswer.CorrectAnswer = request.CorrectAnswer;

   var updatedAnswer = await _questionAnswerRepo.UpdateAsync(existingAnswer);

   if (updatedAnswer == null)
            {
        return null;
   }

   return _mapper.Map<QuestionAnswerResponse>(updatedAnswer);
   }

        public async Task<bool> DeleteQuestionAnswerAsync(Guid id)
 {
   return await _questionAnswerRepo.DeleteAsync(id);
        }
  }
}
