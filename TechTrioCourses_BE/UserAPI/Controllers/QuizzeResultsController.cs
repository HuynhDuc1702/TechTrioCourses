using Microsoft.AspNetCore.Mvc;
using UserAPI.DTOs.Request.QuizzeResult;
using UserAPI.DTOs.Response.QuizzeResult;
using UserAPI.Services.Interfaces;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzeResultsController : ControllerBase
    {
        private readonly IQuizzeResultService _quizzeResultService;

        public QuizzeResultsController(IQuizzeResultService quizzeResultService)
        {
            _quizzeResultService = quizzeResultService;
        }

        // GET: api/QuizzeResults
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizzeResultResponse>>> GetQuizzeResults()
        {
            var results = await _quizzeResultService.GetAllQuizzeResultsAsync();
            return Ok(results);
        }

        // GET: api/QuizzeResults/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizzeResultResponse>> GetQuizzeResult(Guid id)
        {
            var result = await _quizzeResultService.GetQuizzeResultByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/QuizzeResults/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<QuizzeResultResponse>>> GetQuizzeResultsByUser(Guid userId)
        {
            var results = await _quizzeResultService.GetQuizzeResultsByUserIdAsync(userId);
            return Ok(results);
        }

        // GET: api/QuizzeResults/quiz/5
        [HttpGet("quiz/{quizId}")]
        public async Task<ActionResult<IEnumerable<QuizzeResultResponse>>> GetQuizzeResultsByQuiz(Guid quizId)
        {
            var results = await _quizzeResultService.GetQuizzeResultsByQuizIdAsync(quizId);
            return Ok(results);
        }

        // GET: api/QuizzeResults/user/5/quiz/6
        [HttpGet("user/{userId}/quiz/{quizId}")]
        public async Task<ActionResult<IEnumerable<QuizzeResultResponse>>> GetQuizzeResultsByUserAndQuiz(Guid userId, Guid quizId)
        {
            var results = await _quizzeResultService.GetQuizzeResultsByUserAndQuizIdAsync(userId, quizId);
            return Ok(results);
        }

        // PUT: api/QuizzeResults/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuizzeResult(Guid id, UpdateQuizzeResultRequest request)
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
        public async Task<ActionResult<QuizzeResultResponse>> PostQuizzeResult(CreateQuizzeResultRequest request)
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
