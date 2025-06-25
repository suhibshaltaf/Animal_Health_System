using Animal_Health_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IMedicalExaminationRepository
    {
        Task<IEnumerable<MedicalExamination>> GetAllAsync();
        Task<MedicalExamination> GetAsync(int id);
        Task<int> AddAsync(MedicalExamination medicalExamination);
        Task<int> UpdateAsync(MedicalExamination medicalExamination);
        Task DeleteAsync(int id);
    }
}
