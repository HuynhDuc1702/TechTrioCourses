using AutoMapper;
using UserAPI.Application.DTOs.Request.User;
using UserAPI.Application.DTOs.Response.User;
using UserAPI.Application.Interfaces.IRepositories;
using UserAPI.Application.Interfaces.IServices;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepo, IMapper mapper)
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
            var existingUser = await _userRepo.GetByAccountIdAsync(request.AccountId);

            if (existingUser != null)
            {
                return null;
            }

            var user = _mapper.Map<User>(request);
            var createdUser = await _userRepo.CreateAsync(user);

            return _mapper.Map<UserResponse>(createdUser);
        }

        public async Task<UserResponse?> UpdateUserAsync(Guid id, UpdateUserRequest request)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }

          
            _mapper.Map(request, user);

           var updatedUser= await _userRepo.UpdateAsync(user);
            if(updatedUser == null)
            {
                return null;
            }

            return _mapper.Map<UserResponse>(updatedUser);
        }
    }
}
