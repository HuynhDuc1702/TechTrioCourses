using AutoMapper;
using QuizAPI.DTOs.Request.QuizzeResult;
using QuizAPI.DTOs.Response.QuizzeResult;
using QuizAPI.Enums;
using QuizAPI.Models;
using QuizAPI.Repositories.Interfaces;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Services
{
    public class QuizzeResultService : IQuizzeResultService
    {
        private readonly IQuizzeResultRepo _quizzeResultRepo;
     private readonly IMapper _mapper;

public QuizzeResultService(IQuizzeResultRepo quizzeResultRepo, IMapper mapper)
        {
      _quizzeResultRepo = quizzeResultRepo;
   _mapper = mapper;
   }

    public async Task<IEnumerable<QuizzeResultResponse>> GetAllQuizzeResultsAsync()
  {
      var results = await _quizzeResultRepo.GetAllAsync();
    return _mapper.Map<IEnumerable<QuizzeResultResponse>>(results);
     }

  public async Task<QuizzeResultResponse?> GetQuizzeResultByIdAsync(Guid id)
        {
   var result = await _quizzeResultRepo.GetByIdAsync(id);

    if (result == null)
            {
     return null;
  }

       return _mapper.Map<QuizzeResultResponse>(result);
 }

        public async Task<QuizzeResultResponse> CreateQuizzeResultAsync(CreateQuizzeResultRequest request)
        {
            var quizzeResult = _mapper.Map<QuizzeResult>(request);
   quizzeResult.Status = QuizzResultStatusEnum.In_Progress;
     quizzeResult.Score = null;
 quizzeResult.CompletedAt = null;
     quizzeResult.DurationSeconds = null;

var createdResult = await _quizzeResultRepo.CreateAsync(quizzeResult);

    return _mapper.Map<QuizzeResultResponse>(createdResult);
  }

        public async Task<QuizzeResultResponse?> UpdateQuizzeResultAsync(Guid id, UpdateQuizzeResultRequest request)
   {
   var existingResult = await _quizzeResultRepo.GetByIdAsync(id);

   if (existingResult == null)
{
      return null;
      }

// Map only non-null properties from request to existing result
   if (request.Score.HasValue)
      existingResult.Score = request.Score.Value;

     if (request.Status.HasValue)
     existingResult.Status = request.Status.Value;

        if (request.CompletedAt.HasValue)
   existingResult.CompletedAt = request.CompletedAt.Value;

   if (request.DurationSeconds.HasValue)
   existingResult.DurationSeconds = request.DurationSeconds.Value;

if (request.Metadata != null)
    existingResult.Metadata = request.Metadata;

     var updatedResult = await _quizzeResultRepo.UpdateAsync(existingResult);

   if (updatedResult == null)
      {
        return null;
  }

  return _mapper.Map<QuizzeResultResponse>(updatedResult);
        }

        public async Task<bool> DeleteQuizzeResultAsync(Guid id)
 {
      return await _quizzeResultRepo.DeleteAsync(id);
        }
    }
}
