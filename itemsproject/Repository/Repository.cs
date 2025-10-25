using itemsproject.Data;
using itemsproject.Models;
using itemsproject.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace itemsproject.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            this.context = context;
            _dbSet = context.Set<T>();
        }        
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T myItem)
        {
            await _dbSet.AddAsync(myItem);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(T myItem)
        {
            context.Set<T>().Update(myItem);
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var ToDelete = await GetByIdAsync(id);
            if (ToDelete != null)
            {
                context.Set<T>().Remove(ToDelete);
                await context.SaveChangesAsync();
            }
            
        }

        public async Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var inc in includes)
                query = query.Include(inc);
            return await query.ToListAsync();
        }

        // 🔹 Get by Id with optional includes
        public async Task<T?> GetByIdAsync(int id, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _dbSet
                .Where(e => ids.Contains(EF.Property<int>(e, "Id")))
                .ToListAsync();
        }
    }
}
