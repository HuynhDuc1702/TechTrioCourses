using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAPI.DTOs.Request.UserLesson;
using UserAPI.DTOs.Response.UserLesson;
using UserAPI.Services.Interfaces;
using TechTrioCourses.Shared.Enums;
namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLessonsController : ControllerBase
    {
        private readonly IUserLessonService _userLessonService;
        private readonly IUserService _userService;

        public UserLessonsController(IUserLessonService userLessonService, IUserService userService)
        {
            _userLessonService = userLessonService;
            _userService = userService;
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

        // GET: api/UserLessons/by-user
        [HttpGet("by-user")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserLessonResponse>>> GetUserLessonsByUserId()
        {
            // Get AccountId from Token Claims
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var accountGuid))
                return Unauthorized();

            // Resolve User from AccountId
            var user = await _userService.GetUserByAccountIdAsync(accountGuid);
            if (user == null) return Unauthorized();

            var userLessons = await _userLessonService.GetUserLessonsByUserIdAsync(user.Id);
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
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserLessonResponse>>> GetUserLessonsByUserAndCourse(Guid userId, Guid courseId)
        {
            // Get AccountId from Token Claims
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var accountGuid))
                return Unauthorized();

            // Resolve User from AccountId
            var user = await _userService.GetUserByAccountIdAsync(accountGuid);
            if (user == null) return Unauthorized();

            var userLessons = await _userLessonService.GetUserLessonsByUserAndCourseAsync(user.Id, courseId);
            return Ok(userLessons);
        }

        // GET: api/UserLessons/is-completed/{lessonId}
        [HttpGet("is-completed/{lessonId}")]
        [Authorize]
        public async Task<ActionResult> CheckIsCompleted(Guid lessonId)
        {
            // Get AccountId from Token Claims
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var accountGuid))
                return Unauthorized();

            // Resolve User from AccountId
            var user = await _userService.GetUserByAccountIdAsync(accountGuid);
            if (user == null) return Unauthorized();

            var userLesson = await _userLessonService.GetUserLessonByUserAndLessonAsync(user.Id, lessonId);

            return Ok(new { isCompleted = userLesson != null && userLesson.Status == UserLessonStatusEnum.Completed });
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
