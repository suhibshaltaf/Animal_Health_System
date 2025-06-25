using Animal_Health_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IFarmStaffRepository
    {
        Task<IEnumerable<FarmStaff>> GetAllAsync();
        Task<FarmStaff> GetAsync(int id);
        Task<int> AddAsync(FarmStaff  farmStaff);
        Task<int> UpdateAsync(FarmStaff  farmStaff);

        Task DeleteAsync(int id);
        Task<Farm> GetDefaultFarmForStaffAsync();

    }
}
