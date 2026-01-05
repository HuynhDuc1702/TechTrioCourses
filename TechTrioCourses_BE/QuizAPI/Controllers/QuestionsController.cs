using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.DTOs.Request.Question;
using QuizAPI.DTOs.Response.Question;
using QuizAPI.Services;
using QuizAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionResponse>>> GetQuestions()
        {
            var questions = await _questionService.GetAllQuestionsAsync();
            return Ok(questions);
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionResponse>> GetQuestion(Guid id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }
        // GET: api/Questions/5
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<QuestionResponse>> GetQuestionsByCourseId(Guid courseId)
        {
            var questions = await _questionService.GetQuestionCourseIdAsync(courseId);

            if (questions == null)
            {
                return NotFound();
            }

            return Ok(questions);
        }

        // PUT: api/Questions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(Guid id, UpdateQuestionRequest request)
        {
            var updatedQuestion = await _questionService.UpdateQuestionAsync(id, request);

            if (updatedQuestion == null)
            {
                return NotFound();
            }

            return Ok(updatedQuestion);
        }

        // POST: api/Questions
        [HttpPost]
        public async Task<ActionResult<QuestionResponse>> PostQuestion(CreateQuestionRequest request)
        {
            var createdQuestion = await _questionService.CreateQuestionAsync(request);
            return CreatedAtAction(nameof(GetQuestion), new { id = createdQuestion.Id }, createdQuestion);
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var result = await _questionService.DeleteQuestionAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        // PUT: api/Questions/5/disable
        [HttpPut("{id}/disable")]
        public async Task<IActionResult> DisableQuestion(Guid id)
        {
            var result = await _questionService.DisableQuestionAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/Questions/5/archive
        [HttpPut("{id}/archive")]
        public async Task<IActionResult> ArchiveQuestion(Guid id)
        {
            var result = await _questionService.ArchiveQuestionAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
