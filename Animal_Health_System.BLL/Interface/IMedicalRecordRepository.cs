using Animal_Health_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IMedicalRecordRepository
    {
        Task<IEnumerable<MedicalRecord>> GetAllAsync();
        Task<MedicalRecord> GetAsync(int id);
        Task<int> AddAsync(MedicalRecord medicalRecord);
        Task<int> UpdateAsync(MedicalRecord medicalRecord);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
        Task<MedicalRecord> GetByAnimalIdAsync(int animalId);
        Task<IEnumerable<MedicalRecord>> GetByFarmAsync(int farmId);

        Task<bool> AnyAsync(Func<MedicalRecord, bool> predicate);

    }
}
