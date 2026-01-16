using LessonAPI.Datas;
using LessonAPI.Models;
using LessonAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LessonAPI.Repositories
{
    public class LessonRepo : ILessonRepo
    {
        private readonly LessonContext _context;

        public LessonRepo(LessonContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lesson>> GetAllAsync()
        {
            return await _context.Lessons
                
                .ToListAsync();
        }
        public async Task<IEnumerable<Lesson>> GetAllLessonByCourseAsync(Guid courseId)
        {
            return await _context.Lessons
                .Where(l=> l.CourseId == courseId)
                .OrderBy(l=>l.CourseId)
                .ToListAsync();
        }

        public async Task<Lesson?> GetByIdAsync(Guid id)
        {
            return await _context.Lessons
                
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Lesson> CreateAsync(Lesson course)
        {
            course.Id = Guid.NewGuid();
            course.CreatedAt = DateTime.UtcNow;
            course.UpdatedAt = DateTime.UtcNow;

            _context.Lessons.Add(course);
            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<Lesson?> UpdateAsync(Lesson course)
        {
            var existingLesson = await _context.Lessons.FindAsync(course.Id);
            if (existingLesson == null)
            {
                return null;
            }

            course.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingLesson).CurrentValues.SetValues(course);

            try
            {
                await _context.SaveChangesAsync();
                return existingLesson;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(course.Id))
                {
                    return null;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var course = await _context.Lessons.FindAsync(id);
            if (course == null)
            {
                return false;
            }

            _context.Lessons.Remove(course);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Lessons.AnyAsync(e => e.Id == id);
        }
    }
}
