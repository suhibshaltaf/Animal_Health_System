using Animal_Health_System.BLL.Interface;
using Animal_Health_System.DAL.Data;
using Animal_Health_System.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Repository
{
    public class MedicationRepository: IMedicationRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<MedicationRepository> logger;

        public MedicationRepository(ApplicationDbContext context, ILogger<MedicationRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<int> AddAsync(Medication medication)
        {
            try
            {
                bool exists = await context.medications.AnyAsync(m => m.Name == medication.Name && !m.IsDeleted);
                if (exists)
                {
                    throw new Exception("A medication with the same name already exists.");
                }

                await context.medications.AddAsync(medication);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding medication.");
                throw;
            }
        }

        public async Task<IEnumerable<Medication>> GetAllAsync()
        {
            try
            {
                return await context.medications.Where(a => !a.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving medications.");
                throw new Exception("Error occurred while retrieving medications.", ex);
            }
        }

        public async Task<Medication> GetAsync(int id)
        {
            try
            {
                var medication = await context.medications
                    .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

                return medication ?? throw new Exception("Medication not found.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving medication.");
                throw new Exception("Error occurred while retrieving medication.", ex);
            }
        }


        public async Task<int> UpdateAsync(Medication medication)
        {
            try
            {
                bool exists = await context.medications.AnyAsync(m => m.Name == medication.Name && m.Id != medication.Id && !m.IsDeleted);
                if (exists)
                {
                    throw new Exception("A medication with the same name already exists.");
                }

                context.medications.Update(medication);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating medication.");
                throw;
            }
        }
        public async Task DeleteAsync(int id)
        {
            try
            {
                var medication = await context.medications.FirstOrDefaultAsync(m => m.Id == id);
                if (medication == null || medication.IsDeleted)
                {
                    throw new Exception("Medication not found or already deleted.");
                }

                medication.IsDeleted = true;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting medication.");
                throw new Exception("Error occurred while deleting medication.", ex);
            }
        }
        public async Task<IEnumerable<Medication>> FindAsync(Func<Medication, bool> predicate)
        {
            try
            {
                return await Task.Run(() => context.medications.Where(predicate).ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while searching for medications.");
                throw new Exception("Error occurred while searching for medications.", ex);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while saving changes.");
                throw new Exception("Error occurred while saving changes.", ex);
            }
        }
    }
}
