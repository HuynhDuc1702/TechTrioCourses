using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.DTOs.Request.Quiz;
using QuizAPI.DTOs.Response.AttemptQuizDetailDTOs;
using QuizAPI.DTOs.Response.Quiz;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizzesController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        // GET: api/Quizzes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizResponse>>> GetQuizzes()
        {
            var quizzes = await _quizService.GetAllQuizzesAsync();
            return Ok(quizzes);
        }

        // GET: api/Quizzes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizResponse>> GetQuiz(Guid id)
        {
            var quiz = await _quizService.GetQuizByIdAsync(id);

            if (quiz == null)
            {
                return NotFound();
            }

            return Ok(quiz);
        }

        // GET: api/Quizzes/detail/5
        [HttpGet("detail/{id}")]
        public async Task<ActionResult<QuizDetailResponseDto>> GetQuizDetail(Guid id)
        {
            var quiz = await _quizService.GetQuizDetailForAttemptAsync(id);

            if (quiz == null)
            {
                return NotFound();
            }
            return Ok(quiz);
        }

        // GET: api/Quizzes/course/{courseId}
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<QuizResponse>>> GetQuizzesByCourseId(Guid courseId)
        {
            var quizzes = await _quizService.GetQuizzesByCourseIdAsync(courseId);
            return Ok(quizzes);
        }

        // PUT: api/Quizzes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuiz(Guid id, UpdateQuizRequest request)
        {
            var updatedQuiz = await _quizService.UpdateQuizAsync(id, request);

            if (updatedQuiz == null)
            {
                return NotFound();
            }

            return Ok(updatedQuiz);
        }

        // POST: api/Quizzes
        [HttpPost]
        public async Task<ActionResult<QuizResponse>> PostQuiz(CreateQuizRequest request)
        {
            var createdQuiz = await _quizService.CreateQuizAsync(request);
            return CreatedAtAction(nameof(GetQuiz), new { id = createdQuiz.Id }, createdQuiz);
        }

        // DELETE: api/Quizzes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(Guid id)
        {
            var result = await _quizService.DeleteQuizAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/Quizzes/5/disable
        [HttpPut("{id}/disable")]
        public async Task<IActionResult> DisableQuiz(Guid id)
        {
            var result = await _quizService.DisableQuizAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/Quizzes/5/archive
        [HttpPut("{id}/archive")]
        public async Task<IActionResult> ArchiveQuiz(Guid id)
        {
            var result = await _quizService.ArchiveQuizAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
