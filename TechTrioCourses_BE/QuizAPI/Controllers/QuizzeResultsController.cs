using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.DTOs.Request.QuizzeResult;
using QuizAPI.DTOs.Response.QuizzeResult;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Controllers
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
