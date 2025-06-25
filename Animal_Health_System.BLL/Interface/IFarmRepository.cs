using Animal_Health_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IFarmRepository
    {
        Task<IEnumerable<Farm>> GetAllAsync();
        Task<Farm> GetAsync(int id);
        Task<int> AddAsync(Farm farm);
        Task<int> UpdateAsync(Farm farm);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();


    }
}
