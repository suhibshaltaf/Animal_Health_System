using Animal_Health_System.DAL.Models;
using System.Collections.Generic;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.FarmVIMO
{
    public class FarmDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Owner owner { get; set; }  // إظهار اسم المالك

        // Animal-related collections
        public ICollection<Animal> Animals { get; set; } = new List<Animal>();

        // Birth-related collections
        public ICollection<Birth> Births { get; set; } = new List<Birth>();

        // Farm staff
        public ICollection<FarmStaff> FarmStaffs { get; set; } = new List<FarmStaff>();

        // Appointments related to the farm

        // Breeding reports related to the farm

        // Mating related to the farm
        public ICollection<Mating> Matings { get; set; } = new List<Mating>();

        // Farm health summary related to the farm

        // Add IsDeleted field if it's not included already
        public bool IsDeleted { get; set; }  // To display active or deleted status
    }
}
