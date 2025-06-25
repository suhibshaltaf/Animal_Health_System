using Animal_Health_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IVaccineHistoryRepository
    {
        Task<IEnumerable<VaccineHistory>> GetAllAsync();
        Task<VaccineHistory> GetAsync(int id);
        Task<int> AddAsync(VaccineHistory  vaccineHistory);
        Task<int> UpdateAsync(VaccineHistory  vaccineHistory);

        Task DeleteAsync(int id);
        Task SaveChangesAsync();

        Task<IEnumerable<Animal>> GetAnimalsByFarmIdAsync(int farmId);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByAnimalIdAsync(int animalId);
    }
}
