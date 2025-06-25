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
    public class VaccineHistoryRepository : IVaccineHistoryRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<VaccineHistoryRepository> logger;

        public VaccineHistoryRepository(ApplicationDbContext context, ILogger<VaccineHistoryRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<int> AddAsync(VaccineHistory vaccineHistory)
        {
            try
            {
                await context.vaccineHistories.AddAsync(vaccineHistory);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding vaccine history.");
                throw;
            }
        }

        public async Task<int> UpdateAsync(VaccineHistory vaccineHistory)
        {
            try
            {
                context.vaccineHistories.Update(vaccineHistory);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating vaccine history.");
                throw;
            }
        }

        public async Task<IEnumerable<VaccineHistory>> GetAllAsync()
        {
            try
            {
                return await context.vaccineHistories
                    .Include(vh => vh.veterinarian)
                    .Include(vh => vh.vaccine)
                    .Include(vh => vh.medicalRecord)
                    .ThenInclude(mr => mr.Animal) 
                    .ThenInclude(a => a.Farm)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving vaccine histories.");
                throw;
            }
        }

        public async Task<VaccineHistory> GetAsync(int id)
        {
            try
            {
                return await context.vaccineHistories   
                    .Include(vh => vh.veterinarian)
                    .Include(vh => vh.vaccine)
                    .Include(vh => vh.medicalRecord)
                    .ThenInclude(mr => mr.Animal) 
                    .ThenInclude(a => a.Farm) 
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving vaccine history.");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var vaccineHistory = await context.vaccineHistories.FindAsync(id);
                if (vaccineHistory == null)
                    throw new KeyNotFoundException("Vaccine history record not found.");

                vaccineHistory.IsDeleted = true;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting vaccine history.");
                throw;
            }
        }

        public async Task<IEnumerable<Animal>> GetAnimalsByFarmIdAsync(int farmId)
        {
            try
            {
                return await context.animals
                    .Where(a => a.FarmId == farmId && !a.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving animals by farm ID.");
                throw;
            }
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByAnimalIdAsync(int animalId)
        {
            try
            {
                return await context.medicalRecords
                    .Where(mr => mr.AnimalId == animalId && !mr.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving medical records by animal ID.");
                throw;
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
                throw;
            }
        }
    }
}
