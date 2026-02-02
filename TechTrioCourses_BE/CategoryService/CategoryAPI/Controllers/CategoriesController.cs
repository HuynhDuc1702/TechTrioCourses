

using Microsoft.AspNetCore.Mvc;

using CategoryAPI.Application.Interfaces;
using CategoryAPI.Application.DTOs.Response;
using CategoryAPI.Application.DTOs.Request;

namespace CategoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _coursesService;

        public CategoriesController(ICategoryService coursesService)
        {
            _coursesService = coursesService;
        }

        // GET: api/Categorys
        [HttpGet]

        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategorys()
        {
            var courses = await _coursesService.GetAllCategorysAsync();
            return Ok(courses);
        }

        // GET: api/Categorys/5
        [HttpGet("{id}")]

        public async Task<ActionResult<CategoryResponse>> GetCategory(Guid id)
        {
            var course = await _coursesService.GetCategoryByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // POST: api/categories/get-by-ids
        [HttpPost("get-by-ids")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategoriesByIds([FromBody] List<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest(new { message = "Category IDs are required" });
            }

            var categories = await _coursesService.GetCategoriesByIdsAsync(ids);
            return Ok(categories);
        }

        // PUT: api/Categorys/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> PutCategory(Guid id, UpdateCategoryRequest request)
        {
            var updatedCategory = await _coursesService.UpdateCategoryAsync(id, request);

            if (updatedCategory == null)
            {
                return NotFound();
            }

            return Ok(updatedCategory);
        }

        // POST: api/Categorys
        //[Authorize(Roles = "Instructor,Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> PostCategory(CreateCategoryRequest request)
        {
            var createdCategory = await _coursesService.CreateCategoryAsync(request);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
        }

        // DELETE: api/Categorys/5
        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var result = await _coursesService.DeleteCategoryAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
