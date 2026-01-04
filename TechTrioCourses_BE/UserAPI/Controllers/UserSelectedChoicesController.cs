using Microsoft.AspNetCore.Mvc;
using UserAPI.DTOs.Request.UserSelectedChoice;
using UserAPI.DTOs.Response.UserSelectedChoice;
using UserAPI.Services.Interfaces;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSelectedChoicesController : ControllerBase
    {
        private readonly IUserSelectedChoiceService _userSelectedChoiceService;

        public UserSelectedChoicesController(IUserSelectedChoiceService userSelectedChoiceService)
        {
            _userSelectedChoiceService = userSelectedChoiceService;
        }

        // GET: api/UserSelectedChoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSelectedChoiceResponse>>> GetUserSelectedChoices()
        {
            var choices = await _userSelectedChoiceService.GetAllUserSelectedChoicesAsync();
            return Ok(choices);
        }

        // GET: api/UserSelectedChoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserSelectedChoiceResponse>> GetUserSelectedChoice(Guid id)
        {
            var choice = await _userSelectedChoiceService.GetUserSelectedChoiceByIdAsync(id);

            if (choice == null)
            {
                return NotFound();
            }

            return Ok(choice);
        }

        // GET: api/UserSelectedChoices/result/5
        [HttpGet("result/{resultId}")]
        public async Task<ActionResult<IEnumerable<UserSelectedChoiceResponse>>> GetUserSelectedChoicesByResult(Guid resultId)
        {
            var choices = await _userSelectedChoiceService.GetUserSelectedChoicesByResultIdAsync(resultId);
            return Ok(choices);
        }

        // GET: api/UserSelectedChoices/result/5/question/6
        [HttpGet("result/{resultId}/question/{questionId}")]
        public async Task<ActionResult<UserSelectedChoiceResponse>> GetUserSelectedChoiceByResultAndQuestion(Guid resultId, Guid questionId)
        {
            var choice = await _userSelectedChoiceService.GetUserSelectedChoiceByResultAndQuestionIdAsync(resultId, questionId);

            if (choice == null)
            {
                return NotFound();
            }

            return Ok(choice);
        }

        // PUT: api/UserSelectedChoices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserSelectedChoice(Guid id, UpdateUserSelectedChoiceRequest request)
        {
            var updatedChoice = await _userSelectedChoiceService.UpdateUserSelectedChoiceAsync(id, request);

            if (updatedChoice == null)
            {
                return NotFound();
            }

            return Ok(updatedChoice);
        }

        // POST: api/UserSelectedChoices
        [HttpPost]
        public async Task<ActionResult<UserSelectedChoiceResponse>> PostUserSelectedChoice(CreateUserSelectedChoiceRequest request)
        {
            var createdChoice = await _userSelectedChoiceService.CreateUserSelectedChoiceAsync(request);
            return CreatedAtAction(nameof(GetUserSelectedChoice), new { id = createdChoice.Id }, createdChoice);
        }

        // DELETE: api/UserSelectedChoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserSelectedChoice(Guid id)
        {
            var result = await _userSelectedChoiceService.DeleteUserSelectedChoiceAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
