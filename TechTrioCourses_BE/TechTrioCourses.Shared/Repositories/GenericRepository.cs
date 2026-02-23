
using Microsoft.EntityFrameworkCore;
using TechTrioCourses.Shared.Abstractions;
using TechTrioCourses.Shared.Repositories.Interfaces;

namespace TechTrioCourses.Shared.Repositories
{
    public abstract class GenericRepository<T, TContext>(TContext context)
        : IGenericRepository<T>
        where T : BaseEntity
        where TContext : DbContext
    {
        protected readonly TContext _context = context;

        protected abstract DbSet<T> DbSet { get; }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetByIdsAsync(List<Guid> ids)
        {
            return await DbSet
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;

            await DbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<T?> UpdateAsync(T entity)
        {
            var existing = await DbSet.FindAsync(entity.Id);
            if (existing == null) return null;

            entity.UpdatedAt = DateTime.UtcNow;

            _context.Entry(existing).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
            return existing;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity == null) return false;

            DbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> ExistsAsync(Guid id)
        {
            return await DbSet.AnyAsync(e => e.Id == id);
        }
    }
}
