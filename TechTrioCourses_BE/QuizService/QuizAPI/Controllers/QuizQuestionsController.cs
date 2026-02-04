using Microsoft.AspNetCore.Mvc;
using QuizAPI.DTOs.Request.QuizQuestion;
using QuizAPI.DTOs.Response.QuizQuestion;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizQuestionsController : ControllerBase
    {
        private readonly IQuizQuestionService _quizQuestionService;

        public QuizQuestionsController(IQuizQuestionService quizQuestionService)
        {
            _quizQuestionService = quizQuestionService;
        }

        // GET: api/QuizQuestions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizQuestionResponse>>> GetQuizQuestions()
        {
            var quizQuestions = await _quizQuestionService.GetAllQuizQuestionsAsync();
            return Ok(quizQuestions);
        }

        // GET: api/QuizQuestions/{quizId}/{questionId}
        [HttpGet("{quizId}/{questionId}")]
        public async Task<ActionResult<QuizQuestionResponse>> GetQuizQuestion(Guid quizId, Guid questionId)
        {
            var quizQuestion = await _quizQuestionService.GetQuizQuestionByIdAsync(quizId, questionId);

            if (quizQuestion == null)
            {
                return NotFound();
            }

            return Ok(quizQuestion);
        }

        // GET: api/QuizQuestions/quiz/{quizId}
        [HttpGet("quiz/{quizId}")]
        public async Task<ActionResult<IEnumerable<QuizQuestionResponse>>> GetQuizQuestionsByQuizId(Guid quizId)
        {
            var quizQuestions = await _quizQuestionService.GetQuizQuestionsByQuizIdAsync(quizId);
            return Ok(quizQuestions);
        }

        // GET: api/QuizQuestions/question/{questionId}
        [HttpGet("question/{questionId}")]
        public async Task<ActionResult<IEnumerable<QuizQuestionResponse>>> GetQuizQuestionsByQuestionId(Guid questionId)
        {
            var quizQuestions = await _quizQuestionService.GetQuizQuestionsByQuestionIdAsync(questionId);
            return Ok(quizQuestions);
        }

        // POST: api/QuizQuestions
        [HttpPost]
        public async Task<ActionResult<QuizQuestionResponse>> PostQuizQuestion(CreateQuizQuestionRequest request)
        {
            var createdQuizQuestion = await _quizQuestionService.CreateQuizQuestionAsync(request);
            return CreatedAtAction(nameof(GetQuizQuestion),
               new { quizId = createdQuizQuestion.QuizId, questionId = createdQuizQuestion.QuestionId },
               createdQuizQuestion);
        }

        // PUT: api/QuizQuestions/{quizId}/{questionId}
        [HttpPut("{quizId}/{questionId}")]
        public async Task<IActionResult> PutQuizQuestion(Guid quizId, Guid questionId, UpdateQuizQuestionRequest request)
        {
            var updatedQuizQuestion = await _quizQuestionService.UpdateQuizQuestionAsync(quizId, questionId, request);

            if (updatedQuizQuestion == null)
            {
                return NotFound();
            }

            return Ok(updatedQuizQuestion);
        }

        // DELETE: api/QuizQuestions/{quizId}/{questionId}
        [HttpDelete("{quizId}/{questionId}")]
        public async Task<IActionResult> DeleteQuizQuestion(Guid quizId, Guid questionId)
        {
            var result = await _quizQuestionService.DeleteQuizQuestionAsync(quizId, questionId);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
