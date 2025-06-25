using Animal_Health_System.BLL.Interface;
using Animal_Health_System.DAL.Data;
using Animal_Health_System.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Repository
{
    public class PregnancyRepository : IPregnancyRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<PregnancyRepository> logger;

        public PregnancyRepository(ApplicationDbContext context, ILogger<PregnancyRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<int> AddAsync(Pregnancy  pregnancy)
        {
            try
            {
                await context.pregnancies.AddAsync(pregnancy);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding pregnancy.");
                throw new Exception("Error occurred while adding pregnancy.", ex);
            }
        }

        public async Task<IEnumerable<Pregnancy>> GetAllAsync()
        {
            try
            {
                return await context.pregnancies.Include(a=>a.Animal).Include(m=>m.Mating).Where(a => !a.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving pregnancies.");
                throw new Exception("Error occurred while retrieving pregnancies.", ex);
            }
        }

        public async Task<Pregnancy> GetAsync(int id)
        {
            try
            {
                return await context.pregnancies.Include(a => a.Animal).Include(m => m.Mating)
                    .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving pregnancies.");
                throw new Exception("Error occurred while retrieving pregnancies.", ex);
            }
        }

        public async Task<int> UpdateAsync(Pregnancy pregnancies)
        {
            try
            {
                context.pregnancies.Update(pregnancies);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating pregnancies.");
                throw new Exception("Error occurred while updating pregnancies.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var pregnancies = await context.pregnancies.FindAsync(id);
                if (pregnancies != null)
                {
                    pregnancies.IsDeleted = true;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting pregnancies.");
                throw new Exception("Error occurred while deleting pregnancies.", ex);
            }
        }
        public async Task<Pregnancy> FindAsync(Expression<Func<Pregnancy, bool>> predicate)
        {
            try
            {
                return await context.pregnancies.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while searching for pregnancy.");
                throw new Exception("Error occurred while searching for pregnancy.", ex);
            }
        }
    }
}
