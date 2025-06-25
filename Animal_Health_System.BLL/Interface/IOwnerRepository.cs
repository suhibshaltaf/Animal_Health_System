using Animal_Health_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetAllAsync();
        Task<Owner> GetAsync(int id);
        Task<int> AddAsync(Owner owner);
        Task<int> UpdateAsync(Owner owner);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
