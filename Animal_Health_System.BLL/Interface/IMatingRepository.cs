using Animal_Health_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IMatingRepository
    {
        Task<IEnumerable<Mating>> GetAllAsync();
        Task<Mating> GetAsync(int id);
        Task<int> AddAsync(Mating  mating);
        Task<int> UpdateAsync(Mating  mating);
        Task<Mating> GetWithRelationsAsync(int id);
        Task<Mating> FindAsync(Func<Mating, bool> predicate);

        Task DeleteAsync(int id);
    }
}
