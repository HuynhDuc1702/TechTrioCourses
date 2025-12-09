using Microsoft.AspNetCore.Mvc;
using QuizAPI.DTOs.Request.QuestionChoice;
using QuizAPI.DTOs.Response.QuestionChoice;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionChoicesController : ControllerBase
    {
   private readonly IQuestionChoiceService _questionChoiceService;

        public QuestionChoicesController(IQuestionChoiceService questionChoiceService)
  {
     _questionChoiceService = questionChoiceService;
 }

        // GET: api/QuestionChoices
    [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionChoiceResponse>>> GetQuestionChoices()
   {
     var choices = await _questionChoiceService.GetAllQuestionChoicesAsync();
       return Ok(choices);
        }

        // GET: api/QuestionChoices/5
   [HttpGet("{id}")]
  public async Task<ActionResult<QuestionChoiceResponse>> GetQuestionChoice(Guid id)
        {
       var choice = await _questionChoiceService.GetQuestionChoiceByIdAsync(id);

            if (choice == null)
      {
      return NotFound();
    }

       return Ok(choice);
      }

  // GET: api/QuestionChoices/question/5
     [HttpGet("question/{questionId}")]
 public async Task<ActionResult<IEnumerable<QuestionChoiceResponse>>> GetQuestionChoicesByQuestion(Guid questionId)
        {
         var choices = await _questionChoiceService.GetQuestionChoicesByQuestionIdAsync(questionId);
       return Ok(choices);
 }

        // PUT: api/QuestionChoices/5
   [HttpPut("{id}")]
 public async Task<IActionResult> PutQuestionChoice(Guid id, UpdateQuestionChoiceRequest request)
        {
   var updatedChoice = await _questionChoiceService.UpdateQuestionChoiceAsync(id, request);

  if (updatedChoice == null)
   {
    return NotFound();
}

     return Ok(updatedChoice);
        }

        // POST: api/QuestionChoices
     [HttpPost]
  public async Task<ActionResult<QuestionChoiceResponse>> PostQuestionChoice(CreateQuestionChoiceRequest request)
 {
      var createdChoice = await _questionChoiceService.CreateQuestionChoiceAsync(request);
 return CreatedAtAction(nameof(GetQuestionChoice), new { id = createdChoice.Id }, createdChoice);
        }

      // DELETE: api/QuestionChoices/5
     [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteQuestionChoice(Guid id)
 {
  var result = await _questionChoiceService.DeleteQuestionChoiceAsync(id);

         if (!result)
       {
      return NotFound();
     }

   return NoContent();
        }
 }
}
