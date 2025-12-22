using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LessonAPI.Datas;
using LessonAPI.Models;
using LessonAPI.Services.Interfaces;
using LessonAPI.DTOs.Response;
using LessonAPI.DTOs.Request;
using Microsoft.AspNetCore.Authorization;

namespace LessonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonsService;

        public LessonsController(ILessonService lessonsService)
        {
            _lessonsService = lessonsService;
        }

        // GET: api/Lessons
        [HttpGet]

        public async Task<ActionResult<IEnumerable<LessonResponse>>> GetLessons()
        {
            var lessons = await _lessonsService.GetAllLessonsAsync();
            return Ok(lessons);
        }
        // GET: api/Lessons/course/{courseId}
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<LessonResponse>>> GetLessonsByCourse(Guid courseId)
        {
            var lessons = await _lessonsService.GetAllLessonsByCourseAsync(courseId);

            return Ok(lessons);
        }
        // GET: api/Lessons/5
        [HttpGet("{id}")]

        public async Task<ActionResult<LessonResponse>> GetLesson(Guid id)
        {
            var lesson = await _lessonsService.GetLessonByIdAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            return Ok(lesson);
        }

        // PUT: api/Lessons/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> PutLesson(Guid id, UpdateLessonRequest request)
        {
            var updatedLesson = await _lessonsService.UpdateLessonAsync(id, request);

            if (updatedLesson == null)
            {
                return NotFound();
            }

            return Ok(updatedLesson);
        }

        // POST: api/Lessons
        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost]
        public async Task<ActionResult<LessonResponse>> PostLesson(CreateLessonRequest request)
        {
            var createdLesson = await _lessonsService.CreateLessonAsync(request);
            return CreatedAtAction(nameof(GetLesson), new { id = createdLesson.Id }, createdLesson);
        }

        // DELETE: api/Lessons/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLesson(Guid id)
        {
            var result = await _lessonsService.DeleteLessonAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        // DISABLE: api/Lessons/5/disable
        [HttpPut("{id}/disable")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> DisableLesson(Guid id)
        {
            var result = await _lessonsService.DisableLessonAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        // ARCHCHIVE: api/Lessons/5/disable
        [HttpPut("{id}/archive")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveLesson(Guid id)
        {
            var result = await _lessonsService.ArchiveLessonAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

