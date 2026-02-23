using CourseAPI.Application.Interfaces;
using CourseAPI.Domain.Entities;
using CourseAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TechTrioCourses.Shared.Repositories;

namespace CourseAPI.Infrastructure.Repositories
{
    public class CourseRepository : GenericRepository<Course, CourseDbContext>, ICourseRepository
    {

        public CourseRepository(CourseDbContext context)
           : base(context)
        {
        }

        protected override DbSet<Course> DbSet => _context.Courses;

    }
}
