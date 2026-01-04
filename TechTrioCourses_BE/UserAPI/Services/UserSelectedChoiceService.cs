using AutoMapper;
using UserAPI.DTOs.Request.UserSelectedChoice;
using UserAPI.DTOs.Response.UserSelectedChoice;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class UserSelectedChoiceService : IUserSelectedChoiceService
    {
        private readonly IUserSelectedChoiceRepo _userSelectedChoiceRepo;
        private readonly IMapper _mapper;

        public UserSelectedChoiceService(IUserSelectedChoiceRepo userSelectedChoiceRepo, IMapper mapper)
        {
          _userSelectedChoiceRepo = userSelectedChoiceRepo;
       _mapper = mapper;
        }

     public async Task<IEnumerable<UserSelectedChoiceResponse>> GetAllUserSelectedChoicesAsync()
    {
    var choices = await _userSelectedChoiceRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserSelectedChoiceResponse>>(choices);
     }

        public async Task<UserSelectedChoiceResponse?> GetUserSelectedChoiceByIdAsync(Guid id)
        {
   var choice = await _userSelectedChoiceRepo.GetByIdAsync(id);
  if (choice == null)
            {
    return null;
      }
       return _mapper.Map<UserSelectedChoiceResponse>(choice);
        }

        public async Task<IEnumerable<UserSelectedChoiceResponse>> GetUserSelectedChoicesByResultIdAsync(Guid resultId)
        {
    var choices = await _userSelectedChoiceRepo.GetByResultIdAsync(resultId);
       return _mapper.Map<IEnumerable<UserSelectedChoiceResponse>>(choices);
        }

        public async Task<UserSelectedChoiceResponse?> GetUserSelectedChoiceByResultAndQuestionIdAsync(Guid resultId, Guid questionId)
        {
var choice = await _userSelectedChoiceRepo.GetByResultAndQuestionIdAsync(resultId, questionId);
            if (choice == null)
       {
 return null;
  }
            return _mapper.Map<UserSelectedChoiceResponse>(choice);
      }

        public async Task<UserSelectedChoiceResponse> CreateUserSelectedChoiceAsync(CreateUserSelectedChoiceRequest request)
        {
       var userSelectedChoice = _mapper.Map<UserSelectedChoice>(request);
      var createdChoice = await _userSelectedChoiceRepo.CreateAsync(userSelectedChoice);
       return _mapper.Map<UserSelectedChoiceResponse>(createdChoice);
        }

        public async Task<UserSelectedChoiceResponse?> UpdateUserSelectedChoiceAsync(Guid id, UpdateUserSelectedChoiceRequest request)
      {
        var existingChoice = await _userSelectedChoiceRepo.GetByIdAsync(id);
    if (existingChoice == null)
            {
           return null;
       }

     // Map only non-null properties from request to existing choice
 if (request.ChoiceId.HasValue)
 existingChoice.ChoiceId = request.ChoiceId.Value;

     var updatedChoice = await _userSelectedChoiceRepo.UpdateAsync(existingChoice);
            if (updatedChoice == null)
     {
    return null;
  }

     return _mapper.Map<UserSelectedChoiceResponse>(updatedChoice);
}

    public async Task<bool> DeleteUserSelectedChoiceAsync(Guid id)
        {
 return await _userSelectedChoiceRepo.DeleteAsync(id);
        }
    }
}
