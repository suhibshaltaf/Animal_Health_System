using Animal_Health_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IMedicationRepository
    {
        Task<IEnumerable<Medication>> GetAllAsync();
        Task<Medication> GetAsync(int id);
        Task<int> AddAsync(Medication medication);
        Task<int> UpdateAsync(Medication medication);
        Task DeleteAsync(int id);
        Task<IEnumerable<Medication>> FindAsync(Func<Medication, bool> predicate);

        Task SaveChangesAsync();
    }
}
