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
    public class VeterinarianRepository:IVeterinarianRepository{
        private readonly ApplicationDbContext context;
        private readonly ILogger<VeterinarianRepository> logger;

        public VeterinarianRepository(ApplicationDbContext context, ILogger<VeterinarianRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }


        public async Task<int> AddAsync(Veterinarian veterinarian)
        {
            try
            {
                await context.veterinarians.AddAsync(veterinarian);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding a veterinarian.");
                throw new Exception("Error adding veterinarian to the database.", ex);
            }
        }

        public async Task<IEnumerable<Veterinarian>> GetAllAsync()
        {
            try
            {
                return await context.veterinarians
                    .AsNoTracking()
                    .Where(v => !v.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all veterinarians.");
                throw new Exception("Error retrieving veterinarians from the database.", ex);
            }
        }

        public async Task<Veterinarian> GetAsync(int id)
        {
            try
            {
                var veterinarian = await context.veterinarians
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);
                return veterinarian;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving veterinarian with ID {Id}.", id);
                throw new Exception($"Error retrieving veterinarian with ID {id}.", ex);
            }
        }

        public async Task<int> UpdateAsync(Veterinarian veterinarian)
        {
            try
            {
                context.veterinarians.Update(veterinarian);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating veterinarian with ID {Id}.", veterinarian.Id);
                throw new Exception($"Error updating veterinarian with ID {veterinarian.Id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var veterinarian = await context.veterinarians.FindAsync(id);
                if (veterinarian != null)
                {
                    veterinarian.IsDeleted = true;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting veterinarian with ID {Id}.", id);
                throw new Exception($"Error deleting veterinarian with ID {id}.", ex);
            }
        }
       
    }
}
