using Animal_Health_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IPregnancyRepository
    {
        Task<IEnumerable<Pregnancy>> GetAllAsync();
        Task<Pregnancy> GetAsync(int id);
        Task<int> AddAsync(Pregnancy  pregnancy);
        Task<int> UpdateAsync(Pregnancy  pregnancy);

        Task DeleteAsync(int id);
        Task<Pregnancy> FindAsync(Expression<Func<Pregnancy, bool>> predicate);
    }
}
