using CategoryAPI.Infrastructure.Data;
using CategoryAPI.Domain.Entities;
using CategoryAPI.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using TechTrioCourses.Shared.Repositories;

namespace CategoryAPI.Infrastructure.Repositories
{
    public class CategoryRepository :GenericRepository<Category,CategoryDbContext>, ICategoryRepository
    {

        public CategoryRepository(CategoryDbContext context)
             : base(context)
        {
        }

        protected override DbSet<Category> DbSet => _context.Categories;
    }
}
