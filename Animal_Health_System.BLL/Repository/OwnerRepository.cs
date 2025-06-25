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
    public class OwnerRepository : IOwnerRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<OwnerRepository> logger;

        public OwnerRepository(ApplicationDbContext context, ILogger<OwnerRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<int> AddAsync(Owner owner)
        {
            try
            {

                context.owners.Add(owner);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding the owner.");
                throw;
            }
        }

        public async Task<IEnumerable<Owner>> GetAllAsync()
        {
            try
            {
                return await context.owners
                    .Where(o => !o.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching owners.");
                throw;
            }
        }

        public async Task<Owner> GetAsync(int id)
        {
            try
            {
                return await context.owners
                    .Include(o => o.Farms)
                        .ThenInclude(f => f.Animals)
                    .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occurred while fetching the owner with ID {id}.");
                throw;
            }
        }

        public async Task<int> UpdateAsync(Owner owner)
        {
            try
            {
                var existingOwner = await context.owners.FindAsync(owner.Id);
                if (existingOwner == null)
                {
                    throw new KeyNotFoundException("Owner not found");
                }

                existingOwner.FullName = owner.FullName;
                existingOwner.PhoneNumber = owner.PhoneNumber;
                existingOwner.Email = owner.Email;

                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating the owner.");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var owner = await context.owners.FindAsync(id);
                if (owner == null || owner.IsDeleted)
                {
                    throw new KeyNotFoundException($"Owner with ID {id} not found or already deleted.");
                }

                owner.IsDeleted = true;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occurred while deleting the owner with ID {id}.");
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
