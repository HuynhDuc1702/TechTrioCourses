using Microsoft.AspNetCore.Mvc;
using UserAPI.DTOs.Request.UserInputAnswer;
using UserAPI.DTOs.Response.UserInputAnswer;
using UserAPI.Services.Interfaces;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInputAnswersController : ControllerBase
    {
        private readonly IUserInputAnswerService _userInputAnswerService;

        public UserInputAnswersController(IUserInputAnswerService userInputAnswerService)
        {
            _userInputAnswerService = userInputAnswerService;
        }

        // GET: api/UserInputAnswers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInputAnswerResponse>>> GetUserInputAnswers()
        {
            var answers = await _userInputAnswerService.GetAllUserInputAnswersAsync();
            return Ok(answers);
        }

        // GET: api/UserInputAnswers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInputAnswerResponse>> GetUserInputAnswer(Guid id)
        {
            var answer = await _userInputAnswerService.GetUserInputAnswerByIdAsync(id);

            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }

        // GET: api/UserInputAnswers/result/5
        [HttpGet("by-result/{resultId}")]
        public async Task<ActionResult<IEnumerable<UserInputAnswerResponse>>> GetUserInputAnswersByResult(Guid resultId)
        {
            var answers = await _userInputAnswerService.GetUserInputAnswersByResultIdAsync(resultId);
            return Ok(answers);
        }

        // GET: api/UserInputAnswers/result/5/question/6
        [HttpGet("by-result/{resultId}/by-question/{questionId}")]
        public async Task<ActionResult<UserInputAnswerResponse>> GetUserInputAnswerByResultAndQuestion(Guid resultId, Guid questionId)
        {
            var answer = await _userInputAnswerService.GetUserInputAnswerByResultAndQuestionIdAsync(resultId, questionId);

            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }

        // PUT: api/UserInputAnswers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserInputAnswer(Guid id, UpdateUserInputAnswerRequest request)
        {
            var updatedAnswer = await _userInputAnswerService.UpdateUserInputAnswerAsync(id, request);

            if (updatedAnswer == null)
            {
                return NotFound();
            }

            return Ok(updatedAnswer);
        }

        // POST: api/UserInputAnswers
        [HttpPost]
        public async Task<ActionResult<UserInputAnswerResponse>> PostUserInputAnswer(CreateUserInputAnswerRequest request)
        {
            var createdAnswer = await _userInputAnswerService.CreateUserInputAnswerAsync(request);
            return CreatedAtAction(nameof(GetUserInputAnswer), new { id = createdAnswer.Id }, createdAnswer);
        }

        // DELETE: api/UserInputAnswers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserInputAnswer(Guid id)
        {
            var result = await _userInputAnswerService.DeleteUserInputAnswerAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
