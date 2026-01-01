using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserAPI.DTOs.Request;
using UserAPI.DTOs.Response;
using UserAPI.Services.Interfaces;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserQuizzesController : ControllerBase
    {
        private readonly IUserQuizService _userQuizService;
        private readonly IUserService _userService;

        public UserQuizzesController(IUserQuizService userQuizService, IUserService userService)
        {
            _userQuizService = userQuizService;
            _userService = userService;
        }

        // GET: api/UserQuizzes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserQuizResponse>> GetUserQuizById(Guid id)
        {
            var userQuiz = await _userQuizService.GetUserQuizByIdAsync(id);

            if (userQuiz == null)
            {
                return NotFound(new { message = "User quiz not found" });
            }

            return Ok(userQuiz);
        }

        // GET: api/UserQuizzes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserQuizResponse>>> GetAllUserQuizzes()
        {
            var userQuizzes = await _userQuizService.GetAllUserQuizzesAsync();
            return Ok(userQuizzes);
        }

        // GET: api/UserQuizzes/by-user
        [HttpGet("by-user")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserQuizResponse>>> GetUserQuizzesByUserId()
        {
            // Get AccountId from Token Claims
           var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var accountGuid))
                return Unauthorized();

            // Resolve User from AccountId
            var user = await _userService.GetUserByAccountIdAsync(accountGuid);
            if (user == null) return Unauthorized();

            var userQuizzes = await _userQuizService.GetUserQuizzesByUserIdAsync(user.Id);
            return Ok(userQuizzes);
        }

        // GET: api/UserQuizzes/by-quiz/{quizId}
        [HttpGet("by-quiz/{quizId}")]
        public async Task<ActionResult<IEnumerable<UserQuizResponse>>> GetUserQuizzesByQuizId(Guid quizId)
        {
            var userQuizzes = await _userQuizService.GetUserQuizzesByQuizIdAsync(quizId);
            return Ok(userQuizzes);
        }

        // GET: api/UserQuizzes/by-course/{courseId}
        [HttpGet("by-course/{courseId}")]
        public async Task<ActionResult<IEnumerable<UserQuizResponse>>> GetUserQuizzesByCourseId(Guid courseId)
        {
            var userQuizzes = await _userQuizService.GetUserQuizzesByCourseIdAsync(courseId);
            return Ok(userQuizzes);
        }

        // GET: api/UserQuizzes/by-user-and-quiz/{userId}/{quizId}
        [HttpGet("by-user-and-quiz/{quizId}")]
        [Authorize]
        public async Task<ActionResult<UserQuizResponse>> GetUserQuizByUserAndQuiz(Guid quizId)
        {
            // Get AccountId from Token Claims
           var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var accountGuid))
                return Unauthorized();

            // Resolve User from AccountId
            var user = await _userService.GetUserByAccountIdAsync(accountGuid);
            if (user == null) return Unauthorized();

            var userQuiz = await _userQuizService.GetUserQuizByUserAndQuizAsync(user.Id, quizId);

            if (userQuiz == null)
            {
                return NotFound(new { message = "User quiz not found" });
            }

            return Ok(userQuiz);
        }

        // GET: api/UserQuizzes/by-user-and-course/{userId}/{courseId}
        [HttpGet("by-user-and-course/{courseId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserQuizResponse>>> GetUserQuizzesByUserAndCourse(Guid courseId)
        {
            // Get AccountId from Token Claims
           var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var accountGuid))
                return Unauthorized();

            // Resolve User from AccountId
            var user = await _userService.GetUserByAccountIdAsync(accountGuid);
            if (user == null) return Unauthorized();

            var userQuizzes = await _userQuizService.GetUserQuizzesByUserAndCourseAsync(user.Id, courseId);
            return Ok(userQuizzes);
        }

        // POST: api/UserQuizzes
        [HttpPost]
        public async Task<ActionResult<UserQuizResponse>> CreateUserQuiz([FromBody] CreateUserQuizRequest request)
        {
            var userQuiz = await _userQuizService.CreateUserQuizAsync(request);

            if (userQuiz == null)
            {
                return BadRequest(new { message = "User quiz already exists for this user and quiz" });
            }

            return CreatedAtAction(nameof(GetUserQuizById), new { id = userQuiz.Id }, userQuiz);
        }

        // PUT: api/UserQuizzes/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<UserQuizResponse>> UpdateUserQuiz(Guid id, [FromBody] UpdateUserQuizRequest request)
        {
            var userQuiz = await _userQuizService.UpdateUserQuizAsync(id, request);

            if (userQuiz == null)
            {
                return NotFound(new { message = "User quiz not found" });
            }

            return Ok(userQuiz);
        }

        // DELETE: api/UserQuizzes/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserQuiz(Guid id)
        {
            var result = await _userQuizService.DeleteUserQuizAsync(id);

            if (!result)
            {
                return NotFound(new { message = "User quiz not found" });
            }

            return NoContent();
        }
    }
}
