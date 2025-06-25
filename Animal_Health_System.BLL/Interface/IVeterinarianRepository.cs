using Animal_Health_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IVeterinarianRepository
    {
        Task<IEnumerable<Veterinarian>> GetAllAsync();
        Task<Veterinarian> GetAsync(int id);
        Task<int> AddAsync(Veterinarian veterinarian);
        Task<int> UpdateAsync(Veterinarian veterinarian);
        Task DeleteAsync(int id);
    }
}
