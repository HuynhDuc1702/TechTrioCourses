using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CourseAPI.Application.Interfaces;

using CourseAPI.Application.DTOs.Response;
using CourseAPI.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;

namespace CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    

    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _coursesService;

        public CoursesController(ICourseService coursesService)
        {
            _coursesService = coursesService;
        }

    
        // GET: api/Courses
        [HttpGet]

        public async Task<ActionResult<IEnumerable<CourseResponse>>> GetCourses()
        {
            var courses = await _coursesService.GetAllCoursesAsync();
            return Ok(courses);
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
     
        public async Task<ActionResult<CourseResponse>> GetCourse(Guid id)
        {
            var course = await _coursesService.GetCourseByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> PutCourse(Guid id, UpdateCourseRequest request)
        {
            var updatedCourse = await _coursesService.UpdateCourseAsync(id, request);

            if (updatedCourse == null)
            {
                return NotFound();
            }

            return Ok(updatedCourse);
        }

        // POST: api/Courses
        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost]
        public async Task<ActionResult<CourseResponse>> PostCourse(CreateCourseRequest request)
        {
            var createdCourse = await _coursesService.CreateCourseAsync(request);
            return CreatedAtAction(nameof(GetCourse), new { id = createdCourse.Id }, createdCourse);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var result = await _coursesService.DeleteCourseAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        // DISABLE: api/Courses/5/disable
        [HttpPut("{id}/disable")]
       [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> DisableCourse(Guid id)
        {
            var result = await _coursesService.DisableCourseAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        // ARCHCHIVE: api/Courses/5/disable
        [HttpPut("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveCourse(Guid id)
        {
            var result = await _coursesService.ArchiveCourseAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
