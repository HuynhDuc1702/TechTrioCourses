using Microsoft.AspNetCore.Mvc;
using QuizAPI.DTOs.Request.QuestionAnswer;
using QuizAPI.DTOs.Response.QuestionAnswer;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionAnswersController : ControllerBase
    {
        private readonly IQuestionAnswerService _questionAnswerService;

        public QuestionAnswersController(IQuestionAnswerService questionAnswerService)
        {
            _questionAnswerService = questionAnswerService;
        }

        // GET: api/QuestionAnswers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionAnswerResponse>>> GetQuestionAnswers()
        {
            var answers = await _questionAnswerService.GetAllQuestionAnswersAsync();
            return Ok(answers);
        }

        // GET: api/QuestionAnswers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionAnswerResponse>> GetQuestionAnswer(Guid id)
        {
            var answer = await _questionAnswerService.GetQuestionAnswerByIdAsync(id);

            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }

        // GET: api/QuestionAnswers/question/5
        [HttpGet("question/{questionId}")]
        public async Task<ActionResult<IEnumerable<QuestionAnswerResponse>>> GetQuestionAnswersByQuestion(Guid questionId)
        {
            var answers = await _questionAnswerService.GetQuestionAnswersByQuestionIdAsync(questionId);
            return Ok(answers);
        }

        // PUT: api/QuestionAnswers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestionAnswer(Guid id, UpdateQuestionAnswerRequest request)
        {
            var updatedAnswer = await _questionAnswerService.UpdateQuestionAnswerAsync(id, request);

            if (updatedAnswer == null)
            {
                return NotFound();
            }

            return Ok(updatedAnswer);
        }

        // POST: api/QuestionAnswers
        [HttpPost]
        public async Task<ActionResult<QuestionAnswerResponse>> PostQuestionAnswer(CreateQuestionAnswerRequest request)
        {
            var createdAnswer = await _questionAnswerService.CreateQuestionAnswerAsync(request);
            return CreatedAtAction(nameof(GetQuestionAnswer), new { id = createdAnswer.Id }, createdAnswer);
        }

        // DELETE: api/QuestionAnswers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionAnswer(Guid id)
        {
            var result = await _questionAnswerService.DeleteQuestionAnswerAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
