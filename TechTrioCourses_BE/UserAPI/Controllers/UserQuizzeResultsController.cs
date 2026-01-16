using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAPI.DTOs.Request.UserQuizzeResult;
using UserAPI.DTOs.Response.UserQuizzeResult;
using UserAPI.Services;
using UserAPI.Services.Interfaces;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserQuizzeResultsController : ControllerBase
    {
        private readonly IUserQuizzeResultService _quizzeResultService;
        private readonly IUserService _userService;

        public UserQuizzeResultsController(IUserQuizzeResultService quizzeResultService, IUserService userService)
        {
            _quizzeResultService = quizzeResultService;
            _userService = userService;
        }

        // GET: api/QuizzeResults
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserQuizzeResultResponse>>> GetQuizzeResults()
        {
            var results = await _quizzeResultService.GetAllQuizzeResultsAsync();
            return Ok(results);
        }

        // GET: api/QuizzeResults/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserQuizzeResultResponse>> GetQuizzeResult(Guid id)
        {
            var result = await _quizzeResultService.GetQuizzeResultByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/QuizzeResults/by-user/5
        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserQuizzeResultResponse>>> GetQuizzeResultsByUser(Guid userId)
        {
            var results = await _quizzeResultService.GetQuizzeResultsByUserIdAsync(userId);
            return Ok(results);
        }

        // GET: api/QuizzeResults/by-quiz/5
        [HttpGet("by-quiz/{quizId}")]
        public async Task<ActionResult<IEnumerable<UserQuizzeResultResponse>>> GetQuizzeResultsByQuiz(Guid quizId)
        {
            var results = await _quizzeResultService.GetQuizzeResultsByQuizIdAsync(quizId);
            return Ok(results);
        }

        // GET: api/QuizzeResults/by-user-and-quiz/6
        [HttpGet("by-user-and-quiz/{quizId}")]
        public async Task<ActionResult<IEnumerable<UserQuizzeResultResponse>>> GetQuizzeResultsByUserAndQuiz(Guid quizId)
        {
            // Get AccountId from Token Claims
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var accountGuid))
                return Unauthorized();

            // Resolve User from AccountId
            var user = await _userService.GetUserByAccountIdAsync(accountGuid);
            if (user == null) return Unauthorized();

            var results = await _quizzeResultService.GetQuizzeResultsByUserAndQuizIdAsync(user.Id, quizId);
            return Ok(results);
        }

        // GET: api/UserQuizzeResults/by-user-quiz/{userQuizId}
        [HttpGet("by-user-quiz/{userQuizId}")]
        public async Task<ActionResult<IEnumerable<UserQuizzeResultResponse>>> GetQuizzeResultsByUserQuiz(Guid userQuizId)
        {
            var results = await _quizzeResultService.GetQuizzeResultsByUserQuizIdAsync(userQuizId);
            return Ok(results);
        }

        // GET: api/UserQuizzeResults/get-latest/{userQuizId}
        [HttpGet("get-latest/{userQuizId}")]
        public async Task<ActionResult<UserQuizzeResultResponse>> GetLatestQuizzeResultByUserQuiz(Guid userQuizId)
        {
            var results = await _quizzeResultService.GetLatestUserQuizzeResult(userQuizId);
            return Ok(results);
        }

        // PUT: api/QuizzeResults/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuizzeResult(Guid id, UpdateUserQuizzeResultRequest request)
        {
            var updatedResult = await _quizzeResultService.UpdateQuizzeResultAsync(id, request);

            if (updatedResult == null)
            {
                return NotFound();
            }

            return Ok(updatedResult);
        }

        // POST: api/QuizzeResults
        [HttpPost]
        public async Task<ActionResult<UserQuizzeResultResponse>> PostQuizzeResult(CreateUserQuizzeResultRequest request)
        {
            var createdResult = await _quizzeResultService.CreateQuizzeResultAsync(request);
            return CreatedAtAction(nameof(GetQuizzeResult), new { id = createdResult.Id }, createdResult);
        }

        // DELETE: api/QuizzeResults/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuizzeResult(Guid id)
        {
            var result = await _quizzeResultService.DeleteQuizzeResultAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
