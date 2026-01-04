using Microsoft.AspNetCore.Mvc;
using UserAPI.DTOs.Request.User;
using UserAPI.DTOs.Response.User;
using UserAPI.Services.Interfaces;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }
        // GET: api/Users
        [HttpGet]

        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }


        // GET: api/Users/by-account/{accountId}
        [HttpGet("by-account/{accountId}")]
        public async Task<ActionResult<UserResponse>> GetUserByAccountId(Guid accountId)
        {
            var user = await _userService.GetUserByAccountIdAsync(accountId);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        // POST: api/users/get-by-ids
        [HttpPost("get-by-ids")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsersByIds([FromBody] List<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest(new { message = "User IDs are required" });
            }

            var users = await _userService.GetUsersByIdsAsync(ids);
            return Ok(users);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
        {
            var user = await _userService.CreateUserAsync(request);

            if (user == null)
            {
                return BadRequest(new { message = "User already exists for this account" });
            }

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            var user = await _userService.UpdateUserAsync(id, request);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }
    }
}
