using Animal_Health_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IAnimalRepository
    {
        Task<IEnumerable<Animal>> GetAllAsync();
        Task<Animal> GetAsync(int id);
        Task<int> AddAsync(Animal animal);
        Task<int> UpdateAsync(Animal animal);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();

        Task<IEnumerable<Animal>> GetAnimalsByFarmIdAsync(int farmId);


    }
}
