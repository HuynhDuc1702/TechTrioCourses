using AutoMapper;
using QuizAPI.DTOs.Request.UserSelectedChoice;
using QuizAPI.DTOs.Response.UserSelectedChoice;
using QuizAPI.Models;
using QuizAPI.Repositories.Interfaces;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Services
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

        public async Task<UserSelectedChoiceResponse> CreateUserSelectedChoiceAsync(CreateUserSelectedChoiceRequest request)
   {
   var choice = _mapper.Map<Models.UserSelectedChoice>(request);

   var createdChoice = await _userSelectedChoiceRepo.CreateAsync(choice);

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
