using AutoMapper;
using QuizAPI.DTOs.Request.QuestionChoice;
using QuizAPI.DTOs.Response.QuestionChoice;
using QuizAPI.Models;
using QuizAPI.Repositories.Interfaces;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Services
{
    public class QuestionChoiceService : IQuestionChoiceService
 {
        private readonly IQuestionChoiceRepo _questionChoiceRepo;
   private readonly IMapper _mapper;

        public QuestionChoiceService(IQuestionChoiceRepo questionChoiceRepo, IMapper mapper)
   {
    _questionChoiceRepo = questionChoiceRepo;
       _mapper = mapper;
 }

   public async Task<IEnumerable<QuestionChoiceResponse>> GetAllQuestionChoicesAsync()
{
  var choices = await _questionChoiceRepo.GetAllAsync();
      return _mapper.Map<IEnumerable<QuestionChoiceResponse>>(choices);
      }

        public async Task<QuestionChoiceResponse?> GetQuestionChoiceByIdAsync(Guid id)
        {
       var choice = await _questionChoiceRepo.GetByIdAsync(id);

       if (choice == null)
   {
       return null;
 }

  return _mapper.Map<QuestionChoiceResponse>(choice);
        }

        public async Task<IEnumerable<QuestionChoiceResponse>> GetQuestionChoicesByQuestionIdAsync(Guid questionId)
 {
   var choices = await _questionChoiceRepo.GetByQuestionIdAsync(questionId);
 return _mapper.Map<IEnumerable<QuestionChoiceResponse>>(choices);
}

        public async Task<QuestionChoiceResponse> CreateQuestionChoiceAsync(CreateQuestionChoiceRequest request)
        {
 var choice = _mapper.Map<QuestionChoice>(request);

   var createdChoice = await _questionChoiceRepo.CreateAsync(choice);

       return _mapper.Map<QuestionChoiceResponse>(createdChoice);
        }

      public async Task<QuestionChoiceResponse?> UpdateQuestionChoiceAsync(Guid id, UpdateQuestionChoiceRequest request)
        {
       var existingChoice = await _questionChoiceRepo.GetByIdAsync(id);

   if (existingChoice == null)
      {
return null;
       }

    // Map only non-null properties from request to existing choice
      if (request.ChoiceText != null)
         existingChoice.ChoiceText = request.ChoiceText;

            if (request.IsCorrect.HasValue)
        existingChoice.IsCorrect = request.IsCorrect.Value;

       var updatedChoice = await _questionChoiceRepo.UpdateAsync(existingChoice);

            if (updatedChoice == null)
   {
     return null;
 }

    return _mapper.Map<QuestionChoiceResponse>(updatedChoice);
        }

   public async Task<bool> DeleteQuestionChoiceAsync(Guid id)
  {
return await _questionChoiceRepo.DeleteAsync(id);
    }
    }
}
