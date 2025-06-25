using Animal_Health_System.DAL;
using Animal_Health_System.DAL.Models;
using Animal_Health_System.BLL.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animal_Health_System.DAL.Data;

namespace Animal_Health_System.BLL.Repository
{
    public class FarmStaffRepository : IFarmStaffRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<FarmStaffRepository> logger;

        public FarmStaffRepository(ApplicationDbContext context, ILogger<FarmStaffRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<int> AddAsync(FarmStaff farmStaff)
        {
            try
            {
                await context.farmStaff.AddAsync(farmStaff);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding farm staff.");
                throw;
            }
        }

        public async Task<IEnumerable<FarmStaff>> GetAllAsync()
        {
            try
            {
                return await context.farmStaff.Where(a => !a.IsDeleted).Include(f => f.Farm).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving farm staff.");
                throw;
            }
        }

        public async Task<FarmStaff> GetAsync(int id)
        {
            try
            {
                return await context.farmStaff
                    .Include(f => f.Farm)
                    .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted); 
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving farm staff.");
                throw;
            }
        }


        public async Task<int> UpdateAsync(FarmStaff farmStaff)
        {
            try
            {
                context.farmStaff.Update(farmStaff);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating farm staff.");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var farmStaff = await context.farmStaff.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
                if (farmStaff != null)
                {
                    farmStaff.IsDeleted = true;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting farm staff.");
                throw;
            }
        }

        public async Task<Farm> GetDefaultFarmForStaffAsync()
        {
            try
            {
                return await context.farms.FirstOrDefaultAsync(f => f.IsDeleted == true); 
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving the default farm for staff.");
                throw;
            }
        }

    }
}
    