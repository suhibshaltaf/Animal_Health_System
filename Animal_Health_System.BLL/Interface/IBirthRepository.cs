using Animal_Health_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IBirthRepository
    {
        Task<IEnumerable<Birth>> GetAllAsync(string includeProperties = "");
        Task<Birth> GetAsync(int id, string includeProperties = "");
        Task<int> AddAsync(Birth  birth);
        Task<int> UpdateAsync(Birth  birth);
        Task DeleteAsync(int id);
        Task<Birth> GetAsyncByPregnancyId(int pregnancyId); 

    }
}
