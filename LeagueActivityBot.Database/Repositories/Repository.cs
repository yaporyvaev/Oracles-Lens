using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LeagueActivityBot.Database.Repositories
{
    public class Repository<T> : IRepository<T> where T: BaseEntity
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.Set<T>();
        }
        
        public async Task<T> Get(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public IQueryable<T> GetAll(bool includeDeleted)
        {
            var query= _dbSet.AsQueryable();
            
            if (!includeDeleted)
            {
                query = query.Where(e => e.IsDeleted == false);
            }

            return query;
        }

        public async Task<T> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task Update(T entity)
        {
            _appDbContext.Entry(entity).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Remove(T entity)
        {
            entity.IsDeleted = true;
            _appDbContext.Entry(entity).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();
        }
        
        public async Task HardRemove(T entity)
        {
            _dbSet.Remove(entity);
            await _appDbContext.SaveChangesAsync();
        }
    }
}