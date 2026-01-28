using AutoMapper;
using UserAPI.DTOs.Request.UserInputAnswer;
using UserAPI.DTOs.Response.UserInputAnswer;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class UserInputAnswerService : IUserInputAnswerService
    {
        private readonly IUserInputAnswerRepo _userInputAnswerRepo;
        private readonly IMapper _mapper;

        public UserInputAnswerService(IUserInputAnswerRepo userInputAnswerRepo, IMapper mapper)
        {
            _userInputAnswerRepo = userInputAnswerRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserInputAnswerResponse>> GetAllUserInputAnswersAsync()
        {
            var answers = await _userInputAnswerRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserInputAnswerResponse>>(answers);
        }

        public async Task<UserInputAnswerResponse?> GetUserInputAnswerByIdAsync(Guid id)
        {
            var answer = await _userInputAnswerRepo.GetByIdAsync(id);
            if (answer == null)
            {
                return null;
            }
            return _mapper.Map<UserInputAnswerResponse>(answer);
        }

        public async Task<IEnumerable<UserInputAnswerResponse>> GetUserInputAnswersByResultIdAsync(Guid resultId)
        {
            var answers = await _userInputAnswerRepo.GetByResultIdAsync(resultId);
            return _mapper.Map<IEnumerable<UserInputAnswerResponse>>(answers);
        }

        public async Task<UserInputAnswerResponse?> GetUserInputAnswerByResultAndQuestionIdAsync(Guid resultId, Guid questionId)
        {
            var answer = await _userInputAnswerRepo.GetByResultAndQuestionIdAsync(resultId, questionId);
            if (answer == null)
            {
                return null;
            }
            return _mapper.Map<UserInputAnswerResponse>(answer);
        }
        public async Task SaveUserInputAnswer(CreateUserInputAnswerRequest request)
        {
            var existingAnswer = await _userInputAnswerRepo.GetByResultAndQuestionIdAsync(request.ResultId, request.QuestionId);

            if (existingAnswer == null)
            {
                var userInputAnswer = _mapper.Map<UserInputAnswer>(request);
                await _userInputAnswerRepo.CreateAsync(userInputAnswer);
            }
            else
            {

                _mapper.Map(request, existingAnswer);
                await _userInputAnswerRepo.UpdateAsync(existingAnswer);

            }
        }
        public async Task<UserInputAnswerResponse> CreateUserInputAnswerAsync(CreateUserInputAnswerRequest request)
        {
            var userInputAnswer = _mapper.Map<UserInputAnswer>(request);
            var createdAnswer = await _userInputAnswerRepo.CreateAsync(userInputAnswer);
            return _mapper.Map<UserInputAnswerResponse>(createdAnswer);
        }

        public async Task<UserInputAnswerResponse?> UpdateUserInputAnswerAsync(Guid id, UpdateUserInputAnswerRequest request)
        {
            var existingAnswer = await _userInputAnswerRepo.GetByIdAsync(id);
            if (existingAnswer == null)
            {
                return null;
            }

            // Map only non-null properties from request to existing answer
            if (request.AnswerText != null)
                existingAnswer.AnswerText = request.AnswerText;

            await _userInputAnswerRepo.UpdateAsync(existingAnswer);


            return _mapper.Map<UserInputAnswerResponse>(existingAnswer);
        }

        public async Task<bool> DeleteUserInputAnswerAsync(Guid id)
        {
            return await _userInputAnswerRepo.DeleteAsync(id);
        }
    }
}
