using Microsoft.AspNetCore.Mvc;
using UserAPI.DTOs.Request;
using UserAPI.DTOs.Response;
using UserAPI.Services.Interfaces;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCoursesController : ControllerBase
    {
        private readonly IUserCourseService _userCourseService;

        public UserCoursesController(IUserCourseService userCourseService)
        {
            _userCourseService = userCourseService;
        }

        // GET: api/UserCourses/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserCourseResponse>> GetUserCourseById(Guid id)
        {
            var userCourse = await _userCourseService.GetUserCourseByIdAsync(id);

            if (userCourse == null)
            {
                return NotFound(new { message = "User course not found" });
            }

            return Ok(userCourse);
        }

        // GET: api/UserCourses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCourseResponse>>> GetAllUserCourses()
        {
            var userCourses = await _userCourseService.GetAllUserCoursesAsync();
            return Ok(userCourses);
        }

        // GET: api/UserCourses/by-user/{userId}
        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserCourseResponse>>> GetUserCoursesByUserId(Guid userId)
        {
            var userCourses = await _userCourseService.GetUserCoursesByUserIdAsync(userId);
            return Ok(userCourses);
        }

        // GET: api/UserCourses/by-course/{courseId}
        [HttpGet("by-course/{courseId}")]
        public async Task<ActionResult<IEnumerable<UserCourseResponse>>> GetUserCoursesByCourseId(Guid courseId)
        {
            var userCourses = await _userCourseService.GetUserCoursesByCourseIdAsync(courseId);
            return Ok(userCourses);
        }

        // GET: api/UserCourses/by-user-and-course/{userId}/{courseId}
        [HttpGet("by-user-and-course/{userId}/{courseId}")]
        public async Task<ActionResult<UserCourseResponse>> GetUserCourseByUserAndCourse(Guid userId, Guid courseId)
        {
            var userCourse = await _userCourseService.GetUserCourseByUserAndCourseAsync(userId, courseId);

            if (userCourse == null)
            {
                return NotFound(new { message = "User course not found" });
            }

            return Ok(userCourse);
        }

        // POST: api/UserCourses
        [HttpPost]
        public async Task<ActionResult<UserCourseResponse>> CreateUserCourse([FromBody] CreateUserCourseRequest request)
        {
            var userCourse = await _userCourseService.CreateUserCourseAsync(request);

            if (userCourse == null)
            {
                return BadRequest(new { message = "User course already exists for this user and course" });
            }

            return CreatedAtAction(nameof(GetUserCourseById), new { id = userCourse.Id }, userCourse);
        }

        // PUT: api/UserCourses/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<UserCourseResponse>> UpdateUserCourse(Guid id, [FromBody] UpdateUserCourseRequest request)
        {
            var userCourse = await _userCourseService.UpdateUserCourseAsync(id, request);

            if (userCourse == null)
            {
                return NotFound(new { message = "User course not found" });
            }

            return Ok(userCourse);
        }

        // DELETE: api/UserCourses/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserCourse(Guid id)
        {
            var result = await _userCourseService.DeleteUserCourseAsync(id);

            if (!result)
            {
                return NotFound(new { message = "User course not found" });
            }

            return NoContent();
        }
    }
}
