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
    public class AnimalRepository : IAnimalRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<AnimalRepository> logger; 

        public AnimalRepository(ApplicationDbContext context, ILogger<AnimalRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<int> AddAsync(Animal animal)
        {
            try
            {
                await context.animals.AddAsync(animal);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding animal.");
                throw new Exception("Error occurred while adding animal.", ex);
            }
        }

        public async Task<IEnumerable<Animal>> GetAllAsync()
        {
            try
            {
                return await context.animals.Include(f => f.Farm ).Where(a => !a.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving animals.");
                throw new Exception("Error occurred while retrieving animals.", ex);
            }
        }

        public async Task<Animal> GetAsync(int id)
        {
            try
            {
                return await context.animals
                    .Include(f => f.Farm  )
                    .Include(m=>m.MedicalRecord)
                    .Include(m=>m.MedicalExaminations)
                    .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving animal.");
                throw new Exception("Error occurred while retrieving animal.", ex);
            }
        }

        public async Task<int> UpdateAsync(Animal animal)
        {
            try
            {
                context.animals.Update(animal);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating animal.");
                throw new Exception("Error occurred while updating animal.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var animal = await context.animals.FindAsync(id);
                if (animal != null)
                {
                    animal.IsDeleted = true;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting animal.");
                throw new Exception("Error occurred while deleting animal.", ex);
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
                logger.LogError(ex, "Error occurred while retrieving animals for FarmId {FarmId}", farmId);
                throw new Exception($"Error occurred while retrieving animals for FarmId {farmId}.", ex);
            }
        }

    }
}
