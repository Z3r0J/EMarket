using EMarket.Core.Application.Interfaces.Repository;
using EMarket.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMarket.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        public readonly ApplicationContext _applicationContext;
        public GenericRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public virtual async Task<Entity> AddAsync(Entity entity) {

            await _applicationContext.Set<Entity>().AddAsync(entity);
            await _applicationContext.SaveChangesAsync();

            return entity;
        
        }

        public virtual async Task<List<Entity>> GetAllAsync() {

            return await _applicationContext.Set<Entity>().ToListAsync();
        
        }

        public virtual async Task<Entity> GetByIdAsync(int id) {

            return await _applicationContext.Set<Entity>().FindAsync(id);

        }

        public virtual async Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties) {

            var query = _applicationContext.Set<Entity>().AsQueryable();

            foreach (string prop in properties)
            {
                query = query.Include(prop);
            }

            return await query.ToListAsync();

        }

        public virtual async Task UpdateAsync(Entity entity) {

            _applicationContext.Entry(entity).State = EntityState.Modified;

            await _applicationContext.SaveChangesAsync();
        
        }

        public virtual async Task DeleteAsync(Entity entity) {

            _applicationContext.Set<Entity>().Remove(entity);
            await _applicationContext.SaveChangesAsync();

        }

    }
}
