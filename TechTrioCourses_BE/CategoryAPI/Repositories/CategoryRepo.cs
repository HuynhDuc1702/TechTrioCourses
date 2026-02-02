using CategoryAPI.Datas;
using CategoryAPI.Models;
using CategoryAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CategoryAPI.Repositories
{
    public class CategoryRepo: ICategoryRepo
    {
        private readonly CategoryDbContext _context;

        public CategoryRepo(CategoryDbContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories

                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories

                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetByIdsAsync(List<Guid> ids)
        {
            return await _context.Categories
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();
        }

        public async Task<Category> CreateAsync(Category Category)
        {
            Category.Id = Guid.NewGuid();
            
            Category.CreatedAt = DateTime.Now;
            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            return Category;
        }

        public async Task<Category?> UpdateAsync(Category Category)
        {
            var existingCategory = await _context.Categories.FindAsync(Category.Id);
            if (existingCategory == null)
            {
                return null;
            }

           Category.UpdatedAt = DateTime.Now;
            _context.Entry(existingCategory).CurrentValues.SetValues(Category);

            try
            {
                await _context.SaveChangesAsync();
                return existingCategory;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(Category.Id))
                {
                    return null;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var Category = await _context.Categories.FindAsync(id);
            if (Category == null)
            {
                return false;
            }

            _context.Categories.Remove(Category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Categories.AnyAsync(e => e.Id == id);
        }
    }
}
