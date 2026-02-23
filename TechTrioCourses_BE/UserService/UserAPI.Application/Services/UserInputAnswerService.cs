using AutoMapper;
using UserAPI.Application.DTOs.Request.UserInputAnswer;
using UserAPI.Application.DTOs.Response.UserInputAnswer;
using UserAPI.Application.Interfaces.IRepositories;
using UserAPI.Application.Interfaces.IServices;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Services
{
    public class UserInputAnswerService : IUserInputAnswerService
    {
        private readonly IUserInputAnswerRepository _userInputAnswerRepo;
        private readonly IMapper _mapper;

        public UserInputAnswerService(IUserInputAnswerRepository userInputAnswerRepo, IMapper mapper)
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

         
            if (request.AnswerText != null)
                existingAnswer.AnswerText = request.AnswerText;


            var updatedUserInputAnswer = await _userInputAnswerRepo.UpdateAsync(existingAnswer);



            if (updatedUserInputAnswer == null)
            {
                return null;
            }


            return _mapper.Map<UserInputAnswerResponse>(updatedUserInputAnswer);
        }

        public async Task<bool> DeleteUserInputAnswerAsync(Guid id)
        {
            return await _userInputAnswerRepo.DeleteAsync(id);
        }
    }
}
