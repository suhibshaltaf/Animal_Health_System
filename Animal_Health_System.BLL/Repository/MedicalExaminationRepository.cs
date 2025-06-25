using Animal_Health_System.BLL.Interface;
using Animal_Health_System.DAL.Data;
using Animal_Health_System.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Repository
{
    public class MedicalExaminationRepository : IMedicalExaminationRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<MedicalExaminationRepository> logger;

        public MedicalExaminationRepository(ApplicationDbContext context, ILogger<MedicalExaminationRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<int> AddAsync(MedicalExamination medicalExamination)
        {
            try
            {
                await context.medicalExaminations.AddAsync(medicalExamination);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                 logger.LogError(ex, "Error occurred while adding medicalExamination.");
                throw new Exception("Error occurred while adding medicalExamination.", ex);
            }
        }

        public async Task<IEnumerable<MedicalExamination>> GetAllAsync()
        {
            try
            {
                return await context.medicalExaminations
                    .Include(m => m.MedicalRecord)
                    .Include(m => m.Medications)
                    .Include(v => v.Veterinarian)
                    .Include(a => a.Animal)
                    .Where(a => !a.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                 logger.LogError(ex, "Error occurred while retrieving medicalExaminations.");
                throw new Exception("Error occurred while retrieving medicalExaminations.", ex);
            }
        }

        public async Task<MedicalExamination> GetAsync(int id)
        {
            try
            {
                return await context.medicalExaminations
                    .Include(m => m.MedicalRecord)
                    .Include(m => m.Medications)
                    .Include(m => m.Veterinarian)
                    .Include(m => m.Animal)
                    .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
            }
            catch (Exception ex)
            {
                 logger.LogError(ex, "Error occurred while retrieving medicalExamination.");
                throw new Exception("Error occurred while retrieving medicalExamination.", ex);
            }
        }

        public async Task<int> UpdateAsync(MedicalExamination medicalExamination)
        {
            try
            {
                context.medicalExaminations.Update(medicalExamination);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating medicalExamination.");
                throw new Exception("Error occurred while updating medicalExamination.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var medicalExamination = await context.medicalExaminations.FindAsync(id);
                if (medicalExamination != null)
                {
                    medicalExamination.IsDeleted = true;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                 logger.LogError(ex, "Error occurred while deleting medicalExamination.");
                throw new Exception("Error occurred while deleting medicalExamination.", ex);
            }
        }

       
    }
}