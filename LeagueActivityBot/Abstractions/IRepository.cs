using System.Linq;
using System.Threading.Tasks;

namespace LeagueActivityBot.Abstractions
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> Get(int id);
        IQueryable<T> GetAll();
        Task<T> Add(T entity);
        Task Update(T entity);
    }
}