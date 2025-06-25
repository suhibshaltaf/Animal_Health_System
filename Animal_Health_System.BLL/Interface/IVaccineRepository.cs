using Animal_Health_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IVaccineRepository
    {
        Task<IEnumerable<Vaccine>> GetAllAsync();
        Task<Vaccine> GetAsync(int id);
        Task<int> AddAsync(Vaccine vaccine);
        Task<int> UpdateAsync(Vaccine vaccine);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();

        Task<bool> ExistsByNameAsync(string name);

    }
}
