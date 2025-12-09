using CourseAPI.Datas;
using CourseAPI.Models;
using CourseAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseAPI.Repositories
{
    public class CourseRepo : ICourseRepo
    {
        private readonly TechTrioCoursesContext _context;

        public CourseRepo(TechTrioCoursesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses
                
                .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(Guid id)
        {
            return await _context.Courses
                
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course> CreateAsync(Course course)
        {
            course.Id = Guid.NewGuid();
            course.CreatedAt = DateTime.UtcNow;
            course.UpdatedAt = DateTime.UtcNow;

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<Course?> UpdateAsync(Course course)
        {
            var existingCourse = await _context.Courses.FindAsync(course.Id);
            if (existingCourse == null)
            {
                return null;
            }

            course.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingCourse).CurrentValues.SetValues(course);

            try
            {
                await _context.SaveChangesAsync();
                return existingCourse;
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
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return false;
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Courses.AnyAsync(e => e.Id == id);
        }
    }
}
