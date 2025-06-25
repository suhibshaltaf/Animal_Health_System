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
    public class BirthRepository : IBirthRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<BirthRepository> logger;

        public BirthRepository(ApplicationDbContext context, ILogger<BirthRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<int> AddAsync(Birth  birth)
        {
            try
            {
                await context.births.AddAsync(birth);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding birth.");
                throw new Exception("Error occurred while adding birth.", ex);
            }
        }

        public async Task<IEnumerable<Birth>> GetAllAsync(string includeProperties = "")
        {
            try
            {
                IQueryable<Birth> query = context.births.Where(b => !b.IsDeleted);

                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving births.");
                throw new Exception("Error occurred while retrieving births.", ex);
            }
        }


        public async Task<Birth> GetAsync(int id, string includeProperties = "")
        {
            try
            {
                IQueryable<Birth> query = context.births.Where(b => b.Id == id && !b.IsDeleted);

                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving birth.");
                throw new Exception("Error occurred while retrieving birth.", ex);
            }
        }

        public async Task<int> UpdateAsync(Birth  birth)
        {
            try
            {
                context.births.Update(birth);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating birth.");
                throw new Exception("Error occurred while updating birth.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var birth = await context.births.FindAsync(id);
                if (birth != null)
                {
                    birth.IsDeleted = true;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting birth.");
                throw new Exception("Error occurred while deleting birth.", ex);
            }
        }

        public async Task<Birth> GetAsyncByPregnancyId(int pregnancyId)
        {
            try
            {
                return await context.births
                    .Where(b => b.PregnancyId == pregnancyId && !b.IsDeleted)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving birth by PregnancyId.");
                throw new Exception("Error occurred while retrieving birth by PregnancyId.", ex);
            }
        }

    }
}
