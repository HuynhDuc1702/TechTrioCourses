using Microsoft.AspNetCore.Mvc;
using UserAPI.DTOs.Request;
using UserAPI.DTOs.Response;
using UserAPI.Services;
using UserAPI.Services.Interfaces;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLessonsController : ControllerBase
    {
        private readonly IUserLessonService _userLessonService;

        public UserLessonsController(IUserLessonService userLessonService)
        {
            _userLessonService = userLessonService;
        }

        // GET: api/UserLessons/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserLessonResponse>> GetUserLessonById(Guid id)
        {
            var userLesson = await _userLessonService.GetUserLessonByIdAsync(id);

            if (userLesson == null)
            {
                return NotFound(new { message = "User lesson not found" });
            }

            return Ok(userLesson);
        }

        // GET: api/UserLessons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLessonResponse>>> GetAllUserLessons()
        {
            var userLessons = await _userLessonService.GetAllUserLessonsAsync();
            return Ok(userLessons);
        }

        // GET: api/UserLessons/by-user/{userId}
        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserLessonResponse>>> GetUserLessonsByUserId(Guid userId)
        {
            var userLessons = await _userLessonService.GetUserLessonsByUserIdAsync(userId);
            return Ok(userLessons);
        }

        // GET: api/UserLessons/by-lesson/{lessonId}
        [HttpGet("by-lesson/{lessonId}")]
        public async Task<ActionResult<IEnumerable<UserLessonResponse>>> GetUserLessonsByLessonId(Guid lessonId)
        {
            var userLessons = await _userLessonService.GetUserLessonsByLessonIdAsync(lessonId);
            return Ok(userLessons);
        }

        // GET: api/UserLessons/by-course/{courseId}
        [HttpGet("by-course/{courseId}")]
        public async Task<ActionResult<IEnumerable<UserLessonResponse>>> GetUserLessonsByCourseId(Guid courseId)
        {
            var userLessons = await _userLessonService.GetUserLessonsByCourseIdAsync(courseId);
            return Ok(userLessons);
        }

        // GET: api/UserLessons/by-user-and-lesson/{userId}/{lessonId}
        [HttpGet("by-user-and-lesson/{userId}/{lessonId}")]
        public async Task<ActionResult<UserLessonResponse>> GetUserLessonByUserAndLesson(Guid userId, Guid lessonId)
        {
            var userLesson = await _userLessonService.GetUserLessonByUserAndLessonAsync(userId, lessonId);

            if (userLesson == null)
            {
                return NotFound(new { message = "User lesson not found" });
            }

            return Ok(userLesson);
        }

        // GET: api/UserLessons/by-user-and-course/{userId}/{courseId}
        [HttpGet("by-user-and-course/{userId}/{courseId}")]
        public async Task<ActionResult<IEnumerable<UserLessonResponse>>> GetUserLessonsByUserAndCourse(Guid userId, Guid courseId)
        {
            var userLessons = await _userLessonService.GetUserLessonsByUserAndCourseAsync(userId, courseId);
            return Ok(userLessons);
        }

        // GET: api/UserLessons/is-completed/{userId}/{lessonId}
        [HttpGet("is-completed/{userId}/{lessonId}")]
        public async Task<ActionResult> CheckIsCompleted(Guid userId, Guid lessonId)
        {
            var userLesson = await _userLessonService.GetUserLessonByUserAndLessonAsync(userId, lessonId);

            return Ok(new { isCompleted = userLesson != null });
        }

        // POST: api/UserLessons (Creates as completed)
        [HttpPost]
        public async Task<ActionResult<UserLessonResponse>> CreateUserLesson([FromBody] CreateUserLessonRequest request)
        {
            var userLesson = await _userLessonService.CreateUserLessonAsync(request);
            return CreatedAtAction(nameof(GetUserLessonById), new { id = userLesson.Id }, userLesson);
        }

        // DELETE: api/UserLessons/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserLesson(Guid id)
        {
            var result = await _userLessonService.DeleteUserLessonAsync(id);

            if (!result)
            {
                return NotFound(new { message = "User lesson not found" });
            }

            return NoContent();
        }
    }
}
