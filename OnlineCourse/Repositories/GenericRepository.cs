using Microsoft.EntityFrameworkCore;
using OnlineCourse.Data;
using System.Linq.Expressions;

namespace OnlineCourse.Repositories
{
    /// <summary>
    /// Triển khai repository chung cho CRUD + phân trang.
    /// </summary>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"{typeof(TEntity).Name} với ID {id} không tồn tại.");
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate)
            => await _context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<TEntity, bool>>? filter = null)
        {
            var query = _context.Set<TEntity>().AsNoTracking();
            if (filter != null) query = query.Where(filter);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
            => filter == null
                ? await _context.Set<TEntity>().CountAsync()
                : await _context.Set<TEntity>().CountAsync(filter);
    }
}
