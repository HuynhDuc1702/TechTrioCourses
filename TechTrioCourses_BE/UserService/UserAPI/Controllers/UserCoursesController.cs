using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAPI.DTOs.Request.UserCourse;
using UserAPI.DTOs.Response.UserCourse;
using UserAPI.Services.Interfaces;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCoursesController : ControllerBase
    {
        private readonly IUserCourseService _userCourseService;
        private readonly IUserService _userService;

        public UserCoursesController(IUserCourseService userCourseService, IUserService userService)
        {
            _userCourseService = userCourseService;
            _userService= userService;
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

        // GET: api/UserCourses/by-user
        [HttpGet("by-user")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserCourseResponse>>> GetUserCoursesByUserId()
        {
            // Get AccountId from Token Claims
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var accountGuid))
                return Unauthorized();

            // Resolve User from AccountId
            var user = await _userService.GetUserByAccountIdAsync(accountGuid);
            if (user == null) return Unauthorized();

            var userCourses = await _userCourseService.GetUserCoursesByUserIdAsync(user.Id);
            return Ok(userCourses);
        }

        // GET: api/UserCourses/by-course/{courseId}
        [HttpGet("by-course/{courseId}")]
        public async Task<ActionResult<IEnumerable<UserCourseResponse>>> GetUserCoursesByCourseId(Guid courseId)
        {
            var userCourses = await _userCourseService.GetUserCoursesByCourseIdAsync(courseId);
            return Ok(userCourses);
        }

        // GET: api/UserCourses/by-user-and-course/{courseId}
        [HttpGet("by-user-and-course/{courseId}")]
        [Authorize]
        public async Task<ActionResult<UserCourseResponse>> GetUserCourseByUserAndCourse(Guid courseId)
        {
            // Get AccountId from Token Claims
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var accountGuid))
                return Unauthorized();

            // Resolve User from AccountId
            var user = await _userService.GetUserByAccountIdAsync(accountGuid);
            if (user == null) return Unauthorized();

            var userCourse = await _userCourseService.GetUserCourseByUserAndCourseAsync(user.Id, courseId);

            if (userCourse == null)
            {
                return NotFound(new { message = "User course not found" });
            }

            return Ok(userCourse);
        }

        // GET: api/UserCourses/is-enrolled/{courseId}
        [HttpGet("is-enrolled/{courseId}")]
        [Authorize]
        public async Task<ActionResult> CheckIsEnrolled(Guid courseId)
        {
            // Get AccountId from Token Claims
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var accountGuid))
                return Unauthorized();

            // Resolve User from AccountId
            var user = await _userService.GetUserByAccountIdAsync(accountGuid);
            if (user == null) return Unauthorized();

            // Use the resolved UserId
            var userCourse = await _userCourseService.GetUserCourseByUserAndCourseAsync(user.Id, courseId);

            return Ok(new { isEnrolled = userCourse != null });
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
        public async Task<ActionResult<UserCourseResponse>> RecalculateCourseProgress(Guid id)
        {
            var userCourse = await _userCourseService.UpdateUserCourseAsync(id);

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
