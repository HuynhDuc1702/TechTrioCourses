using AutoMapper;
using UserAPI.DTOs.Request;
using UserAPI.DTOs.Response;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserResponse?>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }
        public async Task<UserResponse?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse?> GetUserByAccountIdAsync(Guid accountId)
        {
            var user = await _userRepo.GetByAccountIdAsync(accountId);
            return user == null ? null : _mapper.Map<UserResponse>(user);
        }

        public async Task<IEnumerable<UserResponse>> GetUsersByIdsAsync(List<Guid> ids)
        {
            var users = await _userRepo.GetByIdsAsync(ids);
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse?> CreateUserAsync(CreateUserRequest request)
        {
            // Check if user already exists for this account
            if (await _userRepo.UserExistsAsync(request.AccountId))
            {
                return null;
            }

            // Create user
            var user = _mapper.Map<User>(request);
            var createdUser = await _userRepo.CreateUserAsync(user);

            return _mapper.Map<UserResponse>(createdUser);
        }

        public async Task<UserResponse?> UpdateUserAsync(Guid id, UpdateUserRequest request)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            // Use AutoMapper to update user - only non-null properties will be mapped
            _mapper.Map(request, user);

            await _userRepo.UpdateUserAsync(user);

            return _mapper.Map<UserResponse>(user);
        }
    }
}
