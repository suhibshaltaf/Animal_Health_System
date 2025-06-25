using Animal_Health_System.BLL.Interface;
using Animal_Health_System.DAL.Data;
using Animal_Health_System.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class MedicalRecordRepository : IMedicalRecordRepository
{
    private readonly ApplicationDbContext context;
    private readonly ILogger<MedicalRecordRepository> logger;

    public MedicalRecordRepository(ApplicationDbContext context, ILogger<MedicalRecordRepository> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<int> AddAsync(MedicalRecord medicalRecord)
    {
        try
        {
            await context.medicalRecords.AddAsync(medicalRecord);
            return await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while adding medicalRecord.");
            throw;
        }
    }

    public async Task<IEnumerable<MedicalRecord>> GetAllAsync()
    {
        try
        {
            return await context.medicalRecords
                .Where(a => !a.IsDeleted)
                .Include(m => m.Animal)
                .ThenInclude(a => a.Farm)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving medicalRecords.");
            throw;
        }
    }

    public async Task<MedicalRecord> GetAsync(int id)
    {
        try
        {
            return await context.medicalRecords
                .Include(m => m.Animal)
                .ThenInclude(a => a.Farm) 
                .Include(m => m.Examinations)
                .Include(m => m.vaccineHistories)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving medicalRecord.");
            throw;
        }
    }

    public async Task<int> UpdateAsync(MedicalRecord medicalRecord)
    {
        try
        {
            context.medicalRecords.Update(medicalRecord);
            return await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating medicalRecord.");
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var medicalRecord = await context.medicalRecords.FindAsync(id);
            if (medicalRecord != null)
            {
                medicalRecord.IsDeleted = true;
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting medicalRecord.");
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

    public async Task<MedicalRecord> GetByAnimalIdAsync(int animalId)
    {
        try
        {
            return await context.medicalRecords
                .Include(m => m.Animal)
                .ThenInclude(a => a.Farm)
                .Include(m => m.Examinations)
                .Include(m => m.vaccineHistories)
                .FirstOrDefaultAsync(m => m.AnimalId == animalId && !m.IsDeleted);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving medical record by animal ID.");
            throw;
        }
    }

    public async Task<IEnumerable<MedicalRecord>> GetByFarmAsync(int farmId)
    {
        try
        {
            return await context.medicalRecords
                .Include(m => m.Animal)
                .ThenInclude(a => a.Farm)
                .Where(m => m.Animal.FarmId == farmId && !m.IsDeleted)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving medical records by farm ID.");
            throw;
        }
    }

    public async Task<bool> AnyAsync(Func<MedicalRecord, bool> predicate)
    {
        try
        {
            return await Task.Run(() => context.medicalRecords.Any(predicate));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while checking if any medical record exists.");
            throw;
        }
    }
}
